namespace BrightSmileDentalClinic.ViewModels;

public class AdminDashboardViewModel
{
    public int DentistCount { get; init; }
    public int ServiceCount { get; init; }
    public int PatientCount { get; init; }
    public int PendingAppointmentCount { get; init; }
    public int ApprovedAppointmentCount { get; init; }
    public int CancelledAppointmentCount { get; init; }
    public int ContactMessageCount { get; init; }
}
