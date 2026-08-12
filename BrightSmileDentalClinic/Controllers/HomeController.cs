using System.Diagnostics;
using BrightSmileDentalClinic.Data;
using BrightSmileDentalClinic.Models;
using BrightSmileDentalClinic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrightSmileDentalClinic.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = new HomePageViewModel
        {
            FeaturedServices = await context.Services
                .AsNoTracking()
                .Where(service => service.IsAvailable)
                .OrderBy(service => service.ServiceName)
                .Take(3)
                .ToListAsync(),
            FeaturedDentists = await context.Dentists
                .AsNoTracking()
                .OrderBy(dentist => dentist.LastName)
                .ThenBy(dentist => dentist.FirstName)
                .Take(2)
                .ToListAsync()
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
