using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto.RoleDto
{
    public class RoleSearchFilterDto
    {
        [Required, MinLength(1), MaxLength(40)]
        public string Name { get; set; }

    }
}
