using System.ComponentModel.DataAnnotations;
using BrightSmileDentalClinic.Validation;

namespace BrightSmileDentalClinic.Models;

public class Appointment
{
    public int AppointmentId { get; set; }

    [Required, DataType(DataType.Date), FutureOrToday]
    [Display(Name = "Preferred Date")]
    public DateTime AppointmentDate { get; set; }

    [Required, DataType(DataType.Time)]
    [Display(Name = "Preferred Time")]
    public TimeSpan AppointmentTime { get; set; }

    [Required, StringLength(500)]
    [Display(Name = "Reason for Visit")]
    public string ReasonForVisit { get; set; } = string.Empty;

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

    [Required]
    [Display(Name = "Patient")]
    public int PatientId { get; set; }

    [Required]
    [Display(Name = "Dentist")]
    public int DentistId { get; set; }

    [Required]
    [Display(Name = "Service")]
    public int ServiceId { get; set; }

    [Display(Name = "Submitted")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Patient? Patient { get; set; }
    public Dentist? Dentist { get; set; }
    public Service? Service { get; set; }
}
