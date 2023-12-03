using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GenericBase.Application.Helpers.Attributes
{
    public class EmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return new ValidationResult("Email can not be null!");
            }

            var regex = new Regex(@"^(?!.*[_.-]{2,})(?!.*[_.-]$)(?!.*[_.-]@.*)(?<username>[a-z0-9]+([._-]?[a-z0-9]+)*)@(?<domain>[a-z0-9]+([.-]?[a-z0-9]+)*\.[a-z]{2,3})$", RegexOptions.IgnoreCase);

            if (regex.Match(value.ToString()!).Success)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Enter correct email!");
        }
    }
}
