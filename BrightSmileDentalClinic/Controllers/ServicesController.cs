using BrightSmileDentalClinic.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrightSmileDentalClinic.Controllers;

public class ServicesController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var services = await context.Services
            .AsNoTracking()
            .Where(service => service.IsAvailable)
            .OrderBy(service => service.ServiceName)
            .ToListAsync();

        return View(services);
    }
}
