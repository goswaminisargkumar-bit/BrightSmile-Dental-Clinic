namespace BrightSmileDentalClinic.ViewModels;

public class AppointmentConfirmationViewModel
{
    public int AppointmentId { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public string DentistName { get; init; } = string.Empty;
    public string ServiceName { get; init; } = string.Empty;
    public DateTime AppointmentDate { get; init; }
    public TimeSpan AppointmentTime { get; init; }
}
