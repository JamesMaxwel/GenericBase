using FluentValidation;
using GenericBase.Domain.Entities;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(customer => customer.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(customer => customer.PhoneNumber)
            .NotEmpty().WithMessage("Phone number cannot be empty.")
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");
    }
}
