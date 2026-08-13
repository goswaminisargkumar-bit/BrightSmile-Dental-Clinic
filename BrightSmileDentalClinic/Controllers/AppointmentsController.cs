using System.Data;
using BrightSmileDentalClinic.Data;
using BrightSmileDentalClinic.Models;
using BrightSmileDentalClinic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BrightSmileDentalClinic.Controllers;

public class AppointmentsController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new AppointmentRequestViewModel
        {
            AppointmentDate = DateTime.Today.AddDays(1)
        };

        await PopulateSelectionsAsync(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AppointmentRequestViewModel model)
    {
        ValidateAppointmentDateTime(model);

        var dentist = model.DentistId.HasValue
            ? await context.Dentists.FindAsync(model.DentistId.Value)
            : null;
        var service = model.ServiceId.HasValue
            ? await context.Services.FindAsync(model.ServiceId.Value)
            : null;

        if (model.DentistId.HasValue && dentist is null)
        {
            ModelState.AddModelError(nameof(model.DentistId), "Please select a valid dentist.");
        }

        if (model.ServiceId.HasValue && (service is null || !service.IsAvailable))
        {
            ModelState.AddModelError(nameof(model.ServiceId), "Please select an available dental service.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateSelectionsAsync(model);
            return View(model);
        }

        // A serializable transaction keeps two simultaneous requests from reserving the same dentist slot.
        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var date = model.AppointmentDate!.Value.Date;
        var time = model.AppointmentTime!.Value;
        var dentistId = model.DentistId!.Value;

        var dentistIsBooked = await context.Appointments.AnyAsync(appointment =>
            appointment.DentistId == dentistId
            && appointment.AppointmentDate == date
            && appointment.AppointmentTime == time
            && appointment.Status != AppointmentStatus.Cancelled);

        if (dentistIsBooked)
        {
            await transaction.RollbackAsync();
            ModelState.AddModelError(nameof(model.AppointmentTime),
                "This dentist is already booked at that date and time. Please choose another time.");
            await PopulateSelectionsAsync(model);
            return View(model);
        }

        var normalizedEmail = model.Email.Trim().ToLowerInvariant();
        // Reuse a patient record by normalized email so repeat visitors keep one appointment history.
        var patient = await context.Patients.FirstOrDefaultAsync(existing =>
            existing.Email.ToLower() == normalizedEmail);

        if (patient is null)
        {
            patient = new Patient();
            context.Patients.Add(patient);
        }

        patient.FirstName = model.FirstName.Trim();
        patient.LastName = model.LastName.Trim();
        patient.DateOfBirth = model.DateOfBirth!.Value.Date;
        patient.PhoneNumber = model.PhoneNumber.Trim();
        patient.Email = model.Email.Trim();
        patient.Address = model.Address.Trim();

        var appointment = new Appointment
        {
            AppointmentDate = date,
            AppointmentTime = time,
            ReasonForVisit = model.ReasonForVisit.Trim(),
            Status = AppointmentStatus.Pending,
            Patient = patient,
            DentistId = dentistId,
            ServiceId = model.ServiceId!.Value,
            CreatedDate = DateTime.UtcNow
        };

        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();
        await transaction.CommitAsync();

        // TempData carries confirmation details across the post-redirect-get flow without resubmitting the form.
        TempData["AppointmentId"] = appointment.AppointmentId;
        TempData["PatientName"] = patient.FullName;
        TempData["DentistName"] = dentist!.FullName;
        TempData["ServiceName"] = service!.ServiceName;
        TempData["AppointmentDate"] = appointment.AppointmentDate.ToString("O");
        TempData["AppointmentTime"] = appointment.AppointmentTime.ToString();

        return RedirectToAction(nameof(Confirmation));
    }

    [HttpGet]
    public IActionResult Confirmation()
    {
        if (!int.TryParse(TempData["AppointmentId"]?.ToString(), out var appointmentId)
            || !DateTime.TryParse(TempData["AppointmentDate"]?.ToString(), out var appointmentDate)
            || !TimeSpan.TryParse(TempData["AppointmentTime"]?.ToString(), out var appointmentTime))
        {
            return RedirectToAction(nameof(Create));
        }

        return View(new AppointmentConfirmationViewModel
        {
            AppointmentId = appointmentId,
            PatientName = TempData["PatientName"] as string ?? string.Empty,
            DentistName = TempData["DentistName"] as string ?? string.Empty,
            ServiceName = TempData["ServiceName"] as string ?? string.Empty,
            AppointmentDate = appointmentDate,
            AppointmentTime = appointmentTime
        });
    }

    private void ValidateAppointmentDateTime(AppointmentRequestViewModel model)
    {
        if (model.AppointmentDate.HasValue && model.AppointmentTime.HasValue)
        {
            var requestedAt = model.AppointmentDate.Value.Date.Add(model.AppointmentTime.Value);
            if (requestedAt <= DateTime.Now)
            {
                ModelState.AddModelError(nameof(model.AppointmentTime),
                    "The appointment date and time must be in the future.");
            }
        }
    }

    private async Task PopulateSelectionsAsync(AppointmentRequestViewModel model)
    {
        model.Dentists = await context.Dentists
            .AsNoTracking()
            .OrderBy(dentist => dentist.LastName)
            .ThenBy(dentist => dentist.FirstName)
            .Select(dentist => new SelectListItem
            {
                Value = dentist.DentistId.ToString(),
                Text = $"Dr. {dentist.FirstName} {dentist.LastName} - {dentist.Specialization}"
            })
            .ToListAsync();

        model.Services = await context.Services
            .AsNoTracking()
            .Where(service => service.IsAvailable)
            .OrderBy(service => service.ServiceName)
            .Select(service => new SelectListItem
            {
                Value = service.ServiceId.ToString(),
                Text = $"{service.ServiceName} ({service.DurationMinutes} min)"
            })
            .ToListAsync();
    }
}
