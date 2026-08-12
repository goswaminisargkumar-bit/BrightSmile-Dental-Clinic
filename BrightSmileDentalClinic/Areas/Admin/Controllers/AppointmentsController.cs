using BrightSmileDentalClinic.Data;
using BrightSmileDentalClinic.Infrastructure;
using BrightSmileDentalClinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrightSmileDentalClinic.Areas.Admin.Controllers;

[Area("Admin")]
[AdminAuthorize]
public class AppointmentsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(AppointmentStatus? status)
    {
        var query = context.Appointments
            .AsNoTracking()
            .Include(appointment => appointment.Patient)
            .Include(appointment => appointment.Dentist)
            .Include(appointment => appointment.Service)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(appointment => appointment.Status == status.Value);
        }

        ViewData["SelectedStatus"] = status;
        return View(await query
            .OrderBy(appointment => appointment.AppointmentDate)
            .ThenBy(appointment => appointment.AppointmentTime)
            .ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var appointment = await context.Appointments
            .AsNoTracking()
            .Include(existing => existing.Patient)
            .Include(existing => existing.Dentist)
            .Include(existing => existing.Service)
            .FirstOrDefaultAsync(existing => existing.AppointmentId == id);

        return appointment is null ? NotFound() : View(appointment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var appointment = await context.Appointments
            .Include(existing => existing.Patient)
            .FirstOrDefaultAsync(existing => existing.AppointmentId == id);

        if (appointment is null)
        {
            return NotFound();
        }

        if (appointment.Status != AppointmentStatus.Pending)
        {
            TempData["AdminError"] = "Only pending appointment requests can be approved.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var conflictingApproval = await context.Appointments.AnyAsync(existing =>
            existing.AppointmentId != id
            && existing.DentistId == appointment.DentistId
            && existing.AppointmentDate == appointment.AppointmentDate
            && existing.AppointmentTime == appointment.AppointmentTime
            && existing.Status == AppointmentStatus.Approved);

        if (conflictingApproval)
        {
            TempData["AdminError"] = "Another approved appointment already uses this dentist, date, and time.";
            return RedirectToAction(nameof(Details), new { id });
        }

        appointment.Status = AppointmentStatus.Approved;
        await context.SaveChangesAsync();
        TempData["AdminSuccess"] = $"Appointment #{id} for {appointment.Patient?.FullName} was approved.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var appointment = await context.Appointments
            .Include(existing => existing.Patient)
            .FirstOrDefaultAsync(existing => existing.AppointmentId == id);

        if (appointment is null)
        {
            return NotFound();
        }

        if (appointment.Status != AppointmentStatus.Pending)
        {
            TempData["AdminError"] = "Only pending appointment requests can be cancelled.";
            return RedirectToAction(nameof(Details), new { id });
        }

        appointment.Status = AppointmentStatus.Cancelled;
        await context.SaveChangesAsync();
        TempData["AdminSuccess"] = $"Appointment #{id} for {appointment.Patient?.FullName} was cancelled.";
        return RedirectToAction(nameof(Details), new { id });
    }
}
