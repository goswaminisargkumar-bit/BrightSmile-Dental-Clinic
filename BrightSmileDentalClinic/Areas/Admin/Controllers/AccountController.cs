using BrightSmileDentalClinic.Data;
using BrightSmileDentalClinic.Infrastructure;
using BrightSmileDentalClinic.Models;
using BrightSmileDentalClinic.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrightSmileDentalClinic.Areas.Admin.Controllers;

[Area("Admin")]
public class AccountController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (HttpContext.Session.GetInt32(AdminSession.AdminId).HasValue)
        {
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        return View(new AdminLoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(AdminLoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var normalizedUsername = model.Username.Trim().ToLowerInvariant();
        var admin = await context.Admins.FirstOrDefaultAsync(existing =>
            existing.Username.ToLower() == normalizedUsername);

        if (admin is null)
        {
            AddInvalidLoginError();
            return View(model);
        }

        var passwordHasher = new PasswordHasher<BrightSmileDentalClinic.Models.Admin>();
        var result = passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, model.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            AddInvalidLoginError();
            return View(model);
        }

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            admin.PasswordHash = passwordHasher.HashPassword(admin, model.Password);
            await context.SaveChangesAsync();
        }

        HttpContext.Session.Clear();
        HttpContext.Session.SetInt32(AdminSession.AdminId, admin.AdminId);
        HttpContext.Session.SetString(AdminSession.AdminName, admin.FullName);

        if (Url.IsLocalUrl(model.ReturnUrl))
        {
            return LocalRedirect(model.ReturnUrl);
        }

        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }

    private void AddInvalidLoginError()
    {
        ModelState.AddModelError(string.Empty, "The username or password is incorrect.");
    }
}
