using BrightSmileDentalClinic.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrightSmileDentalClinic.Controllers;

public class DentistsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var dentists = await context.Dentists
            .AsNoTracking()
            .OrderBy(dentist => dentist.LastName)
            .ThenBy(dentist => dentist.FirstName)
            .ToListAsync();

        return View(dentists);
    }
}
