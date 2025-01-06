using GenericBase.Application.Dto.PermissionDto;
using GenericBase.Application.Dto.RoleDto;

namespace GenericBase.Application.Dto.AccountDto
{
    public class AccountCredentialResponseDto
    {
        public ICollection<RoleResponseDto> Roles { get; set; } = [];
        public ICollection<PermissionResponseDto> Permissions { get; set; } = [];
    }
}
