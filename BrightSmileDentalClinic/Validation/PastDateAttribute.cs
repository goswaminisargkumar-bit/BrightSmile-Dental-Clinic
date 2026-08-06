using System.ComponentModel.DataAnnotations;

namespace BrightSmileDentalClinic.Validation;

public sealed class PastDateAttribute : ValidationAttribute
{
    public PastDateAttribute()
    {
        ErrorMessage = "{0} must be in the past.";
    }

    public override bool IsValid(object? value)
    {
        return value is not DateTime date || date.Date < DateTime.Today;
    }
}
