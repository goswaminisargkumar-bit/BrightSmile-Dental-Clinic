using System.ComponentModel.DataAnnotations;
using BrightSmileDentalClinic.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrightSmileDentalClinic.ViewModels;

public class AppointmentRequestViewModel
{
    [Required, StringLength(50)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required, DataType(DataType.Date), PastDate]
    [Display(Name = "Date of Birth")]
    public DateTime? DateOfBirth { get; set; }

    [Required, Phone, StringLength(25)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(250)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Dentist")]
    public int? DentistId { get; set; }

    [Required]
    [Display(Name = "Dental Service")]
    public int? ServiceId { get; set; }

    [Required, DataType(DataType.Date), FutureOrToday]
    [Display(Name = "Preferred Date")]
    public DateTime? AppointmentDate { get; set; }

    [Required, DataType(DataType.Time)]
    [Display(Name = "Preferred Time")]
    public TimeSpan? AppointmentTime { get; set; }

    [Required, StringLength(500)]
    [Display(Name = "Reason for Visit")]
    public string ReasonForVisit { get; set; } = string.Empty;

    public IEnumerable<SelectListItem> Dentists { get; set; } = [];
    public IEnumerable<SelectListItem> Services { get; set; } = [];
}
