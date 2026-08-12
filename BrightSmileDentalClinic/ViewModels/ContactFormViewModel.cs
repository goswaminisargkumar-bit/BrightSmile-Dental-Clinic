using System.ComponentModel.DataAnnotations;

namespace BrightSmileDentalClinic.ViewModels;

public class ContactFormViewModel
{
    [Required, StringLength(100)]
    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(100)]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Phone, StringLength(25)]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Required, StringLength(150)]
    public string Subject { get; set; } = string.Empty;

    [Required, StringLength(2000, MinimumLength = 10)]
    public string Message { get; set; } = string.Empty;
}
