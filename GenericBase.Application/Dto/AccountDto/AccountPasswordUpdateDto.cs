using GenericBase.Application.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto.AccountDto
{
    public class AccountPasswordUpdateDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required, Password]
        public string NewPassword { get; set; } = string.Empty;
    }
}
