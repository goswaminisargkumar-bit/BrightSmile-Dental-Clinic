using BrightSmileDentalClinic.Data;
using BrightSmileDentalClinic.Models;
using BrightSmileDentalClinic.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BrightSmileDentalClinic.Controllers;

public class ContactController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new ContactFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var contactMessage = new ContactMessage
        {
            Name = model.Name.Trim(),
            Email = model.Email.Trim(),
            PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? null : model.PhoneNumber.Trim(),
            Subject = model.Subject.Trim(),
            Message = model.Message.Trim(),
            SubmittedDate = DateTime.UtcNow
        };

        context.ContactMessages.Add(contactMessage);
        await context.SaveChangesAsync();

        TempData["ContactSuccess"] =
            "Thank you for contacting BrightSmile. Our team will respond as soon as possible.";

        return RedirectToAction(nameof(Index));
    }
}
