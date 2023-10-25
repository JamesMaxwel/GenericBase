using GenericBase.Application.Dto.PermissionDto;

namespace GenericBase.Application.Dto.RoleDto
{
    public class RoleResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<PermissionResponseDto> Permissions { get; set; }
    }

}
