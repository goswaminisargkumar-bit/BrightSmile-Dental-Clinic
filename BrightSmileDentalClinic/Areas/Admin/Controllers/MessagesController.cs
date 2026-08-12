using BrightSmileDentalClinic.Data;
using BrightSmileDentalClinic.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrightSmileDentalClinic.Areas.Admin.Controllers;

[Area("Admin")]
[AdminAuthorize]
public class MessagesController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await context.ContactMessages
            .AsNoTracking()
            .OrderByDescending(message => message.SubmittedDate)
            .ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var message = await context.ContactMessages.AsNoTracking()
            .FirstOrDefaultAsync(existing => existing.ContactMessageId == id);
        return message is null ? NotFound() : View(message);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var message = await context.ContactMessages.AsNoTracking()
            .FirstOrDefaultAsync(existing => existing.ContactMessageId == id);
        return message is null ? NotFound() : View(message);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var message = await context.ContactMessages.FindAsync(id);
        if (message is null)
        {
            return NotFound();
        }

        context.ContactMessages.Remove(message);
        await context.SaveChangesAsync();
        TempData["AdminSuccess"] = $"Message from {message.Name} was deleted.";
        return RedirectToAction(nameof(Index));
    }
}
