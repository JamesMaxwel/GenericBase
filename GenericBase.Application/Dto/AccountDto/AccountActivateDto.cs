using GenericBase.Application.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto.AccountDto
{
    public class AccountActivateDto
    {
        [Required, Email]
        public string Email { get; set; } = string.Empty;
        [Required, Number]
        public int Code { get; set; }
    }
}
