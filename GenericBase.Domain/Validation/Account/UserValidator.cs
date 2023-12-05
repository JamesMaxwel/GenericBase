using FluentValidation;
using GenericBase.Domain.Entities.Account;

namespace GenericBase.Domain.Validation.Account
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(user => user.PasswordHash)
                .NotEmpty().WithMessage("Password is required.")
                .Must(ValidatePassword).WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");

            RuleFor(user => user.Salt)
                .NotEmpty().WithMessage("Salt is required.");

            RuleFor(user => user.LockoutEnd)
                .Must(BeInFuture).WithMessage("LockoutEnd must be in the future.");

            RuleFor(user => user.Type)
                .IsInEnum().WithMessage("Invalid user type.");

            RuleFor(user => user.Description)
                .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.");
        }

        private bool BeInFuture(DateTimeOffset? lockoutEnd)
        {
            return lockoutEnd == null || lockoutEnd > DateTimeOffset.UtcNow;
        }

        private bool ValidatePassword(string passwordHash)
        {
            // Enforce a minimum length of 8 characters
            if (passwordHash.Length < 8)
                return false;

            // Require at least one uppercase letter
            if (!passwordHash.Any(char.IsUpper))
                return false;

            // Require at least one lowercase letter
            if (!passwordHash.Any(char.IsLower))
                return false;

            // Require at least one digit
            if (!passwordHash.Any(char.IsDigit))
                return false;

            // Require at least one special character
            if (!passwordHash.Any(ch => !char.IsLetterOrDigit(ch)))
                return false;

            return true;
        }
    }
}
