using GenericBase.Domain.Entities.Account;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto.RoleDto
{
    public class RoleCreateDto
    {
        [Required, MinLength(5), MaxLength(40)]
        public string Name { get; set; } = string.Empty;

        [Required, MinLength(5), MaxLength(40)]
        string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public ICollection<Guid> ClaimsIds { get; set; } = new HashSet<Guid>();

        public static implicit operator Role(RoleCreateDto dto)
        {
            return new Role(dto.Name, dto.Slug, dto.Description);

        }
    }

}
