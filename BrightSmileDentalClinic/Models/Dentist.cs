using System.ComponentModel.DataAnnotations;

namespace BrightSmileDentalClinic.Models;

public class Dentist
{
    public int DentistId { get; set; }

    [Required, StringLength(50)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Specialization { get; set; } = string.Empty;

    [Required, Phone, StringLength(25)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Range(0, 70)]
    [Display(Name = "Years of Experience")]
    public int YearsOfExperience { get; set; }

    [Required, StringLength(200)]
    public string Availability { get; set; } = string.Empty;

    [StringLength(255)]
    [Display(Name = "Profile Image")]
    public string? ImageFileName { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    [Display(Name = "Dentist")]
    public string FullName => $"{FirstName} {LastName}";
}
