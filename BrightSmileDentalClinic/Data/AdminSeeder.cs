using BrightSmileDentalClinic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BrightSmileDentalClinic.Data;

public static class AdminSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        await using var scope = services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (await context.Admins.AnyAsync())
        {
            // Seed only the first administrator; existing accounts must never be overwritten at startup.
            return;
        }

        var username = configuration["AdminSeed:Username"];
        var password = configuration["AdminSeed:Password"];
        var fullName = configuration["AdminSeed:FullName"] ?? "Clinic Administrator";

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            // Credentials are supplied through user secrets or environment configuration, not source control.
            return;
        }

        var admin = new Admin
        {
            Username = username.Trim(),
            FullName = fullName.Trim()
        };

        var passwordHasher = new PasswordHasher<Admin>();
        // Store a one-way hash so the database never contains the administrator's plaintext password.
        admin.PasswordHash = passwordHasher.HashPassword(admin, password);

        context.Admins.Add(admin);
        await context.SaveChangesAsync();
    }
}
