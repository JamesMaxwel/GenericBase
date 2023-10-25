using GenericBase.Application.Dto.PermissionDto;
using GenericBase.Application.Dto.RoleDto;

namespace GenericBase.Application.Dto.AccountDto
{
    public class AccountCredentialResponseDto
    {
        public ICollection<RoleResponseDto> Roles { get; set; } = new List<RoleResponseDto>();
        public ICollection<PermissionResponseDto> Permissions { get; set; } = new List<PermissionResponseDto>();
    }
}
