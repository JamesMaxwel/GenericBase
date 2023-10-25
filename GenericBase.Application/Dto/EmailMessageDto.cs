using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto
{
    public class EmailMessageDto
    {
        [Required]
        public string To { get; set; } = string.Empty;
        [Required]
        public string Body { get; set; } = string.Empty;
        [Required]
        public string Subject { get; set; } = string.Empty;
    }
}
