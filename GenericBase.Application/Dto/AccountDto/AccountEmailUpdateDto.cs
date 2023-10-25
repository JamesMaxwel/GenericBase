using GenericBase.Application.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto.AccountDto
{
    public class AccountEmailUpdateDto
    {
        [Required, Email]
        public string CurrentEmail { get; set; } = string.Empty;
        [Required, Email]
        public string NewEmail { get; set; } = string.Empty;
        [Required, Number]
        public int NewEmailCode { get; set; }
        [Required, Password]
        public string Password { get; set; } = string.Empty;

    }
}
