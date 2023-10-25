using GenericBase.Application.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto.AccountDto
{
    public class AccountLoginDto
    {
        [Required, Email]
        public string Email { get; set; } = string.Empty;
        [Required, Password]
        public string Password { get; set; } = string.Empty;
    }
}
