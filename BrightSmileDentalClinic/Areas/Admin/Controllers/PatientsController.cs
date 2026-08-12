using BrightSmileDentalClinic.Data;
using BrightSmileDentalClinic.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrightSmileDentalClinic.Areas.Admin.Controllers;

[Area("Admin")]
[AdminAuthorize]
public class PatientsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(string? search)
    {
        var query = context.Patients.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(patient =>
                patient.FirstName.Contains(term)
                || patient.LastName.Contains(term)
                || patient.Email.Contains(term)
                || patient.PhoneNumber.Contains(term));
        }

        ViewData["Search"] = search;
        return View(await query
            .Include(patient => patient.Appointments)
            .OrderBy(patient => patient.LastName)
            .ThenBy(patient => patient.FirstName)
            .ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var patient = await context.Patients
            .AsNoTracking()
            .Include(existing => existing.Appointments)
                .ThenInclude(appointment => appointment.Dentist)
            .Include(existing => existing.Appointments)
                .ThenInclude(appointment => appointment.Service)
            .FirstOrDefaultAsync(existing => existing.PatientId == id);

        if (patient is null)
        {
            return NotFound();
        }

        patient.Appointments = patient.Appointments
            .OrderByDescending(appointment => appointment.AppointmentDate)
            .ThenByDescending(appointment => appointment.AppointmentTime)
            .ToList();

        return View(patient);
    }
}
