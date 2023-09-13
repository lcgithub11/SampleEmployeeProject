using System.ComponentModel.DataAnnotations;

public class DateOfBirthAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (!(value is DateTime date))
        {
            return new ValidationResult("Invalid birthdate format.");
        }

        if (date > DateTime.Now)
        {
            return new ValidationResult("Birthdate cannot be in the future.");
        }

        if (DateTime.Now.Year - date.Year < 18)
        {
            return new ValidationResult("Employee must be older than 18 years.");
        }

        return ValidationResult.Success;
    }
}
