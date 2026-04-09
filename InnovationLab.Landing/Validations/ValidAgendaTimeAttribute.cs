using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Models;

namespace InnovationLab.Landing.Validations;

public class ValidAgendaTimeAttribute : ValidationAttribute
{
    private static readonly TimeOnly MinTime = new(5, 0);  // 5:00 AM
    private static readonly TimeOnly MaxTime = new(19, 0); // 7:00 PM

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is not AgendaItem item)
            return ValidationResult.Success;

        if (item.StartTime < MinTime)
            return new ValidationResult("StartTime must be at or after 5:00 AM");
        if (item.EndTime > MaxTime)
            return new ValidationResult("EndTime must be at or before 7:00 PM");
        if (item.EndTime <= item.StartTime)
            return new ValidationResult("EndTime must be after StartTime.");

        return ValidationResult.Success;
    }
}