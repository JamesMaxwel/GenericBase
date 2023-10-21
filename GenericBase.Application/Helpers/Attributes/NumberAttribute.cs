using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Helpers.Attributes
{
    public class NumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            string numberValue = value as string;

            foreach (var item in numberValue)
            {
                if (!char.IsDigit(item))
                {
                    return new ValidationResult("Enter the information as a number.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
