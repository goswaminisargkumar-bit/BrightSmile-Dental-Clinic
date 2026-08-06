using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightSmileDentalClinic.Models;

public class Service
{
    public int ServiceId { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Service Name")]
    public string ServiceName { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(1, 480)]
    [Display(Name = "Duration (minutes)")]
    public int DurationMinutes { get; set; }

    [Range(typeof(decimal), "0", "100000")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [StringLength(255)]
    [Display(Name = "Service Image")]
    public string? ImageFileName { get; set; }

    [Display(Name = "Available")]
    public bool IsAvailable { get; set; } = true;

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
