using BrightSmileDentalClinic.Models;

namespace BrightSmileDentalClinic.ViewModels;

public class HomePageViewModel
{
    public IReadOnlyList<Service> FeaturedServices { get; init; } = [];
    public IReadOnlyList<Dentist> FeaturedDentists { get; init; } = [];
}
