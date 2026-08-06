using BrightSmileDentalClinic.Models;
using Microsoft.EntityFrameworkCore;

namespace BrightSmileDentalClinic.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Dentist> Dentists => Set<Dentist>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Dentist>().HasIndex(d => d.Email).IsUnique();
        modelBuilder.Entity<Service>().HasIndex(s => s.ServiceName).IsUnique();
        modelBuilder.Entity<Patient>().HasIndex(p => p.Email);
        modelBuilder.Entity<Admin>().HasIndex(a => a.Username).IsUnique();

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Dentist)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DentistId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Service)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasIndex(a => new { a.DentistId, a.AppointmentDate, a.AppointmentTime });

        modelBuilder.Entity<Dentist>().HasData(
            new Dentist
            {
                DentistId = 1,
                FirstName = "Emily",
                LastName = "Carter",
                Specialization = "General Dentistry",
                PhoneNumber = "519-555-0101",
                Email = "emily.carter@brightsmile.example",
                YearsOfExperience = 12,
                Availability = "Monday to Friday, 9:00 AM to 5:00 PM"
            },
            new Dentist
            {
                DentistId = 2,
                FirstName = "Daniel",
                LastName = "Lee",
                Specialization = "Orthodontics",
                PhoneNumber = "519-555-0102",
                Email = "daniel.lee@brightsmile.example",
                YearsOfExperience = 9,
                Availability = "Tuesday to Saturday, 10:00 AM to 6:00 PM"
            });

        modelBuilder.Entity<Service>().HasData(
            new Service
            {
                ServiceId = 1,
                ServiceName = "Dental Cleaning",
                Description = "Professional cleaning to remove plaque and maintain oral health.",
                DurationMinutes = 45,
                Price = 120m,
                IsAvailable = true
            },
            new Service
            {
                ServiceId = 2,
                ServiceName = "Teeth Whitening",
                Description = "In-clinic whitening treatment for a brighter smile.",
                DurationMinutes = 60,
                Price = 299m,
                IsAvailable = true
            },
            new Service
            {
                ServiceId = 3,
                ServiceName = "Dental Filling",
                Description = "Tooth-coloured filling used to repair cavities and minor damage.",
                DurationMinutes = 60,
                Price = 180m,
                IsAvailable = true
            });
    }
}
