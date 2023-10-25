using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto.AccountDto
{
    public class AccountUpdateDto
    {
        [Required, MinLength(1), MaxLength(40)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MinLength(1), MaxLength(40)]
        public string LastName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
