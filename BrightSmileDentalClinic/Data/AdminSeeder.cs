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
            return;
        }

        var username = configuration["AdminSeed:Username"];
        var password = configuration["AdminSeed:Password"];
        var fullName = configuration["AdminSeed:FullName"] ?? "Clinic Administrator";

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        var admin = new Admin
        {
            Username = username.Trim(),
            FullName = fullName.Trim()
        };

        var passwordHasher = new PasswordHasher<Admin>();
        admin.PasswordHash = passwordHasher.HashPassword(admin, password);

        context.Admins.Add(admin);
        await context.SaveChangesAsync();
    }
}
