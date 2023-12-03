using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto.PermissionDto
{
    public class PermissionSearchFilterDto
    {
        [Required, MinLength(1), MaxLength(40)]
        public string Key { get; set; } = string.Empty;

        public string? Value { get; set; }

    }
}
