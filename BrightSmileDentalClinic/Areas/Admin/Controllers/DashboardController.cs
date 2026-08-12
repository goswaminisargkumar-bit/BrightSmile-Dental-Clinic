using BrightSmileDentalClinic.Data;
using BrightSmileDentalClinic.Infrastructure;
using BrightSmileDentalClinic.Models;
using BrightSmileDentalClinic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrightSmileDentalClinic.Areas.Admin.Controllers;

[Area("Admin")]
[AdminAuthorize]
public class DashboardController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = new AdminDashboardViewModel
        {
            DentistCount = await context.Dentists.CountAsync(),
            ServiceCount = await context.Services.CountAsync(),
            PatientCount = await context.Patients.CountAsync(),
            PendingAppointmentCount = await context.Appointments.CountAsync(appointment =>
                appointment.Status == AppointmentStatus.Pending),
            ApprovedAppointmentCount = await context.Appointments.CountAsync(appointment =>
                appointment.Status == AppointmentStatus.Approved),
            CancelledAppointmentCount = await context.Appointments.CountAsync(appointment =>
                appointment.Status == AppointmentStatus.Cancelled),
            ContactMessageCount = await context.ContactMessages.CountAsync()
        };

        return View(model);
    }
}
