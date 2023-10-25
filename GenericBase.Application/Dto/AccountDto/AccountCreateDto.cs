using GenericBase.Application.Helpers.Attributes;
using GenericBase.Domain.Entities.Account;
using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto.AccountDto
{
    public class AccountCreateDto
    {
        [Required, MinLength(1), MaxLength(40)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MinLength(1), MaxLength(40)]
        public string LastName { get; set; } = string.Empty;
        [Required, Email]
        public string Email { get; set; } = string.Empty;
        [Required, Password]
        public string Password { get; set; } = string.Empty;

        public static implicit operator User(AccountCreateDto dto)
            => new(dto.Email, dto.FirstName, dto.LastName, dto.Password);

    }
}