using System.ComponentModel.DataAnnotations;

namespace BrightSmileDentalClinic.Models;

public class ContactMessage
{
    public int ContactMessageId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Phone, StringLength(25)]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Required, StringLength(150)]
    public string Subject { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    public string Message { get; set; } = string.Empty;

    [Display(Name = "Submitted")]
    public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;
}
