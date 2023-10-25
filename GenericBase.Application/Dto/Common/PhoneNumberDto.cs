using GenericBase.Application.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto.Common
{
    public class PhoneNumberDto
    {
        [Required(ErrorMessage = "PhoneNumber is required!"), PhoneNumber]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
