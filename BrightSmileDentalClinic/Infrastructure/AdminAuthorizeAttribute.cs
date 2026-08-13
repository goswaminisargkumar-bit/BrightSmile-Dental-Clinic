using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BrightSmileDentalClinic.Infrastructure;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AdminAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Session.GetInt32(AdminSession.AdminId).HasValue)
        {
            return;
        }

        // Preserve the requested address so a successful login can return staff to their original page.
        var returnUrl = $"{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}";
        context.Result = new RedirectToActionResult(
            "Login",
            "Account",
            new { area = "Admin", returnUrl });
    }
}
