using GenericBase.Application.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto.AccountDto
{
    public class AccountResetPasswordDto
    {
        [Required, Email]
        public string Email { get; set; } = string.Empty;
        [Required, Number]
        public int ResetCode { get; set; }
        [Required, Password]
        public string NewPassword { get; set; } = string.Empty;
    }
}
