using BrightSmileDentalClinic.Data;
using BrightSmileDentalClinic.Infrastructure;
using BrightSmileDentalClinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrightSmileDentalClinic.Areas.Admin.Controllers;

[Area("Admin")]
[AdminAuthorize]
public class ServicesController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var services = await context.Services
            .AsNoTracking()
            .Include(service => service.Appointments)
            .OrderBy(service => service.ServiceName)
            .ToListAsync();

        return View(services);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Service { IsAvailable = true, DurationMinutes = 30 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("ServiceName,Description,DurationMinutes,Price,ImageFileName,IsAvailable")] Service service)
    {
        await ValidateUniqueNameAsync(service.ServiceName);

        if (!ModelState.IsValid)
        {
            return View(service);
        }

        Normalize(service);
        context.Services.Add(service);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(nameof(service.ServiceName), "A service with this name already exists.");
            return View(service);
        }

        TempData["AdminSuccess"] = $"{service.ServiceName} was added successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var service = await context.Services.FindAsync(id);
        return service is null ? NotFound() : View(service);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("ServiceId,ServiceName,Description,DurationMinutes,Price,ImageFileName,IsAvailable")] Service service)
    {
        if (id != service.ServiceId)
        {
            return BadRequest();
        }

        if (!await context.Services.AnyAsync(existing => existing.ServiceId == id))
        {
            return NotFound();
        }

        await ValidateUniqueNameAsync(service.ServiceName, id);

        if (!ModelState.IsValid)
        {
            return View(service);
        }

        Normalize(service);
        context.Update(service);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await context.Services.AnyAsync(existing => existing.ServiceId == id))
            {
                return NotFound();
            }

            throw;
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(nameof(service.ServiceName), "A service with this name already exists.");
            return View(service);
        }

        TempData["AdminSuccess"] = $"{service.ServiceName} was updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var service = await context.Services
            .AsNoTracking()
            .Include(existing => existing.Appointments)
            .FirstOrDefaultAsync(existing => existing.ServiceId == id);

        return service is null ? NotFound() : View(service);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var service = await context.Services.FindAsync(id);
        if (service is null)
        {
            return NotFound();
        }

        if (await context.Appointments.AnyAsync(appointment => appointment.ServiceId == id))
        {
            TempData["AdminError"] =
                $"{service.ServiceName} cannot be deleted because appointment records use this service.";
            return RedirectToAction(nameof(Index));
        }

        context.Services.Remove(service);
        await context.SaveChangesAsync();
        TempData["AdminSuccess"] = $"{service.ServiceName} was deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidateUniqueNameAsync(string name, int? excludedId = null)
    {
        var normalizedName = name.Trim().ToLowerInvariant();
        var exists = await context.Services.AnyAsync(existing =>
            existing.ServiceName.ToLower() == normalizedName
            && (!excludedId.HasValue || existing.ServiceId != excludedId.Value));

        if (exists)
        {
            ModelState.AddModelError(nameof(Service.ServiceName), "A service with this name already exists.");
        }
    }

    private static void Normalize(Service service)
    {
        service.ServiceName = service.ServiceName.Trim();
        service.Description = service.Description.Trim();
        service.ImageFileName = string.IsNullOrWhiteSpace(service.ImageFileName)
            ? null
            : service.ImageFileName.Trim();
    }
}
