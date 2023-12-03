using GenericBase.Application.Dto.PermissionDto;
using System.Collections.ObjectModel;


namespace GenericBase.Application.Dto.RoleDto
{
    public class RoleResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<PermissionResponseDto> Permissions { get; set; } = new Collection<PermissionResponseDto>();
    }

}
