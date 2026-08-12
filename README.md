# BrightSmile Dental Clinic

BrightSmile Dental Clinic is an ASP.NET Core MVC web application for browsing dental services and dentists, submitting appointment requests, and managing clinic operations through a planned admin portal.

This project is being developed for the PROG8771 Advanced Programming with .NET final group project.

## Current features

- Responsive public website built with Bootstrap
- Database-driven dental service listing
- Database-driven dentist listing
- Patient appointment-request form
- Dentist and service dropdowns populated from the database
- Patient reuse based on email address
- Appointment date and time validation
- Dentist scheduling-conflict prevention
- Pending appointment confirmation page
- SQL Server database integration using Entity Framework Core
- Initial dentist and service seed data

## Technology stack

- ASP.NET Core MVC
- .NET 10
- C#
- Razor views
- Entity Framework Core 10.0.10
- SQL Server LocalDB
- Bootstrap
- JavaScript and jQuery validation
- Git and GitHub

## Project structure

```text
BrightSmile-Dental-Clinic/
├── .config/                         # Project-local .NET tools
├── BrightSmileDentalClinic.slnx     # Visual Studio solution
└── BrightSmileDentalClinic/
    ├── Controllers/                 # MVC request handlers
    ├── Data/                        # EF Core database context
    ├── Migrations/                  # Database migration history
    ├── Models/                      # Database entities and validation
    ├── Validation/                  # Custom validation attributes
    ├── ViewModels/                  # Page-specific form and display models
    ├── Views/                       # Razor pages
    ├── wwwroot/                     # CSS, JavaScript, and static assets
    ├── appsettings.json             # Application and database settings
    └── Program.cs                   # Application startup configuration
```

## Database models

- `Dentist`
- `Service`
- `Patient`
- `Appointment`
- `Admin`
- `ContactMessage`

An appointment connects one patient, one dentist, and one dental service. A patient, dentist, or service can be associated with multiple appointments.

## Prerequisites

Install the following before running the project:

- Visual Studio with the ASP.NET and web development workload, or the .NET 10 SDK
- SQL Server Express LocalDB
- Git

## Getting started

1. Clone the repository:

   ```powershell
   git clone https://github.com/goswaminisargkumar-bit/BrightSmile-Dental-Clinic.git
   cd BrightSmile-Dental-Clinic
   ```

2. Restore the application packages and project-local EF Core tool:

   ```powershell
   dotnet restore BrightSmileDentalClinic.slnx
   dotnet tool restore
   ```

3. Create or update the LocalDB database:

   ```powershell
   dotnet tool run dotnet-ef database update `
     --project BrightSmileDentalClinic/BrightSmileDentalClinic.csproj `
     --startup-project BrightSmileDentalClinic/BrightSmileDentalClinic.csproj
   ```

4. Run the application:

   ```powershell
   dotnet run --project BrightSmileDentalClinic/BrightSmileDentalClinic.csproj
   ```

You can also open `BrightSmileDentalClinic.slnx` in Visual Studio and run the HTTPS profile.

## Configuration

The development database uses SQL Server LocalDB with this database name:

```text
BrightSmileDentalClinic
```

The connection string is stored in `BrightSmileDentalClinic/appsettings.json`. Do not commit production passwords, private connection strings, or other secrets to this repository.

## Planned work

- Contact page and database-backed contact form
- Enhanced home page with featured services and dentists
- Admin login and protected admin area
- Admin dashboard
- Dentist and service CRUD operations
- Appointment approval and cancellation
- Patient and contact-message management
- Final accessibility, responsive-design, and validation testing
- Project documentation and screenshots

## Team workflow

Before starting work, synchronize your local repository:

```powershell
git checkout master
git pull origin master
```

Create a separate branch for each feature:

```powershell
git checkout -b feature/short-feature-name
```

After completing and testing the feature:

```powershell
git add .
git commit -m "Describe the completed feature"
git push -u origin feature/short-feature-name
```

Open a pull request on GitHub and ask another team member to review it before merging into `master`. Avoid committing `.vs`, `bin`, `obj`, `.user`, or `.suo` files.

## Build verification

Run the following before opening a pull request:

```powershell
dotnet build BrightSmileDentalClinic.slnx
```

The current project builds successfully with zero warnings and zero errors.
