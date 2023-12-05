using FluentValidation;
using GenericBase.Domain.Entities.Account;

namespace GenericBase.Domain.Validation.Account
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(role => role.Name)
                .NotEmpty().WithMessage("The name is required.")
                .Length(3, 50).WithMessage("The name must be between 3 and 50 characters.");

            RuleFor(role => role.Slug)
                .NotEmpty().WithMessage("The slug is required.")
                .Matches(@"^[a-zA-Z0-9.]+$").WithMessage("The Slug can only contain letters, numbers, and dots, and cannot have spaces.")
                .Length(3, 50).WithMessage("The slug must be between 3 and 50 characters.");

            RuleFor(role => role.Description)
                .MaximumLength(255).WithMessage("The description cannot exceed 255 characters.");

            RuleFor(role => role.Permissions)
                .Must(p => p.Count > 0).WithMessage("At least one permission is required.");
        }


    }
}
