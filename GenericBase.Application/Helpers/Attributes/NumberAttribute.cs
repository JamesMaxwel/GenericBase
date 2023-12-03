using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Helpers.Attributes
{
    public class NumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value is null)
                return new ValidationResult("Number can not be null!");


            if (value is not string numberValue)
                return new ValidationResult("The value must be a string.");


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
