using System.ComponentModel.DataAnnotations;

namespace BrightSmileDentalClinic.Validation;

public sealed class FutureOrTodayAttribute : ValidationAttribute
{
    public FutureOrTodayAttribute()
    {
        ErrorMessage = "{0} cannot be in the past.";
    }

    public override bool IsValid(object? value)
    {
        return value is not DateTime date || date.Date >= DateTime.Today;
    }
}
