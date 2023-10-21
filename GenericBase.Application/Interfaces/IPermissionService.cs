using GenericBase.Application.Dto.PermissionDto;
using GenericBase.Domain.Entities.Account;
using System.Linq.Expressions;

namespace GenericBase.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<PermissionResponseDto> GetByIdAsync(Guid permissionId);
        Task<bool> CreateAsync(PermissionCreateDto permissionCreateDto);
        Task<bool> DeleteAsync(Guid permissionId);
        Task<ICollection<PermissionResponseDto>?> GetAllAsync();
        Task<ICollection<PermissionResponseDto>?> GetWhereAsync(Expression<Func<Permission, bool>> predicate);
    }
}
