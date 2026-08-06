using System.ComponentModel.DataAnnotations;
using BrightSmileDentalClinic.Validation;

namespace BrightSmileDentalClinic.Models;

public class Patient
{
    public int PatientId { get; set; }

    [Required, StringLength(50)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required, DataType(DataType.Date), PastDate]
    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    [Required, Phone, StringLength(25)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(250)]
    public string Address { get; set; } = string.Empty;

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    [Display(Name = "Patient")]
    public string FullName => $"{FirstName} {LastName}";
}
