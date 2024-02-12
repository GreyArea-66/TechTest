using System.ComponentModel.DataAnnotations;
using System;

/// <summary>
/// Ensures a date is not in the future.
/// </summary>
public class PastDateAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates that the date is not in the future.
    /// </summary>
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateOnly date)
        {
            if (date > DateOnly.FromDateTime(DateTime.Today))
            {
                return new ValidationResult("Date of Birth cannot be a future date.");
            }
        }

        return ValidationResult.Success!;
    }
}