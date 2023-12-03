using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GenericBase.Application.Helpers.Attributes
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("Enter correct PhoneNumber!");
            }

            var regex = new Regex(@"^(\d{2})(\d{5}|\d{4})(\d{4})$");

            return regex.Match(value?.ToString()!).Success ? ValidationResult.Success
                : new ValidationResult("Please enter valid phone number. Phone must be contains only numbers!");
        }
    }
}

