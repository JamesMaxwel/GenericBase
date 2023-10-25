using GenericBase.Application.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto.Common
{
    public class EmailAdressDto
    {
        [Required(ErrorMessage = "Email is required!"), Email]
        public string Email { get; set; } = string.Empty;

    }
}
