using System.ComponentModel.DataAnnotations;

namespace BrightSmileDentalClinic.Models;

public class Admin
{
    public int AdminId { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;
}
