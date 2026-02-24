using System.ComponentModel.DataAnnotations;

namespace GameTournamentApi.Validation;

public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // 1. Om värdet är null, låt [Required] hantera det istället.
        if (value == null)
        {
            return ValidationResult.Success;
        }

        // 2. Vi kollar om värdet faktiskt är ett datum
        if (value is DateTime date)
        {
            // 3. Om datumet är "mindre än nu" (dvs i dåtiden) -> Fel!
            if (date < DateTime.Now)
            {
                return new ValidationResult("Date cannot be in the past! Please travel back to the future. 🏎️💨");
            }
        }

        // 4. Allt ok!
        return ValidationResult.Success;
    }
}