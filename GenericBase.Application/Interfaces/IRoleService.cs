using GenericBase.Application.Dto.RoleDto;
using GenericBase.Domain.Entities.Account;
using System.Linq.Expressions;

namespace GenericBase.Application.Interfaces
{
    public interface IRoleService
    {
        Task<RoleResponseDto> GetByIdAsync(Guid roleId);
        Task<bool> CreateAsync(RoleCreateDto roleCreateDto);
        Task<bool> UpdateAsync(Guid roleId, RoleUpdateDto roleUpdateDto);
        Task<bool> DeleteAsync(Guid roleId);

        Task<ICollection<RoleResponseDto>?> GetAllAsync();
        Task<ICollection<RoleResponseDto>?> GetWhereAsync(Expression<Func<Role, bool>> predicate);
    }
}
