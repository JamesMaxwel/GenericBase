using FluentValidation;
using GenericBase.Domain.Entities.Account;

namespace GenericBase.Domain.Validation.Account
{
    public class PermissionValidator : AbstractValidator<Permission>
    {
        public PermissionValidator()
        {
            RuleFor(permission => permission.Key)
                .NotEmpty().WithMessage("The key is required.")
                .Matches(@"^[a-zA-Z0-9.]+$").WithMessage("The key can only contain letters, numbers, and dots, and cannot have spaces.")
                .Length(3, 50).WithMessage("The key must be between 3 and 50 characters.");

            RuleFor(permission => permission.Value)
                .NotEmpty().WithMessage("The value is required.")
                .MaximumLength(100).WithMessage("The value cannot exceed 100 characters.");
        }
    }
}
