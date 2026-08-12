using BrightSmileDentalClinic.Data;
using BrightSmileDentalClinic.Infrastructure;
using BrightSmileDentalClinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrightSmileDentalClinic.Areas.Admin.Controllers;

[Area("Admin")]
[AdminAuthorize]
public class DentistsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var dentists = await context.Dentists
            .AsNoTracking()
            .Include(dentist => dentist.Appointments)
            .OrderBy(dentist => dentist.LastName)
            .ThenBy(dentist => dentist.FirstName)
            .ToListAsync();

        return View(dentists);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Dentist());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("FirstName,LastName,Specialization,PhoneNumber,Email,YearsOfExperience,Availability,ImageFileName")] Dentist dentist)
    {
        await ValidateUniqueEmailAsync(dentist.Email);

        if (!ModelState.IsValid)
        {
            return View(dentist);
        }

        Normalize(dentist);
        context.Dentists.Add(dentist);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(nameof(dentist.Email), "A dentist with this email address already exists.");
            return View(dentist);
        }

        TempData["AdminSuccess"] = $"Dr. {dentist.FullName} was added successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dentist = await context.Dentists.FindAsync(id);
        return dentist is null ? NotFound() : View(dentist);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("DentistId,FirstName,LastName,Specialization,PhoneNumber,Email,YearsOfExperience,Availability,ImageFileName")] Dentist dentist)
    {
        if (id != dentist.DentistId)
        {
            return BadRequest();
        }

        if (!await context.Dentists.AnyAsync(existing => existing.DentistId == id))
        {
            return NotFound();
        }

        await ValidateUniqueEmailAsync(dentist.Email, id);

        if (!ModelState.IsValid)
        {
            return View(dentist);
        }

        Normalize(dentist);
        context.Update(dentist);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await context.Dentists.AnyAsync(existing => existing.DentistId == id))
            {
                return NotFound();
            }

            throw;
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(nameof(dentist.Email), "A dentist with this email address already exists.");
            return View(dentist);
        }

        TempData["AdminSuccess"] = $"Dr. {dentist.FullName} was updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var dentist = await context.Dentists
            .AsNoTracking()
            .Include(existing => existing.Appointments)
            .FirstOrDefaultAsync(existing => existing.DentistId == id);

        return dentist is null ? NotFound() : View(dentist);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var dentist = await context.Dentists.FindAsync(id);
        if (dentist is null)
        {
            return NotFound();
        }

        if (await context.Appointments.AnyAsync(appointment => appointment.DentistId == id))
        {
            TempData["AdminError"] =
                $"Dr. {dentist.FullName} cannot be deleted because appointment records use this dentist.";
            return RedirectToAction(nameof(Index));
        }

        context.Dentists.Remove(dentist);
        await context.SaveChangesAsync();
        TempData["AdminSuccess"] = $"Dr. {dentist.FullName} was deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidateUniqueEmailAsync(string email, int? excludedId = null)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var exists = await context.Dentists.AnyAsync(existing =>
            existing.Email.ToLower() == normalizedEmail
            && (!excludedId.HasValue || existing.DentistId != excludedId.Value));

        if (exists)
        {
            ModelState.AddModelError(nameof(Dentist.Email), "A dentist with this email address already exists.");
        }
    }

    private static void Normalize(Dentist dentist)
    {
        dentist.FirstName = dentist.FirstName.Trim();
        dentist.LastName = dentist.LastName.Trim();
        dentist.Specialization = dentist.Specialization.Trim();
        dentist.PhoneNumber = dentist.PhoneNumber.Trim();
        dentist.Email = dentist.Email.Trim();
        dentist.Availability = dentist.Availability.Trim();
        dentist.ImageFileName = string.IsNullOrWhiteSpace(dentist.ImageFileName)
            ? null
            : dentist.ImageFileName.Trim();
    }
}
