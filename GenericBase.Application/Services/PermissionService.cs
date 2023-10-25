using AutoMapper;
using GenericBase.Application.Dto.PermissionDto;
using GenericBase.Application.Helpers.Exceptions;
using GenericBase.Application.Interfaces;
using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.Interfaces.Common;
using System.Linq.Expressions;
using System.Net;

namespace GenericBase.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PermissionResponseDto> GetByIdAsync(Guid permissionId)
        {
            var permission = await _unitOfWork.Permissions.GetFirstOrDefaultAsync(permissionId)
              ?? throw new StatusCodeException(HttpStatusCode.NotFound, "Not found");

            return _mapper.Map<PermissionResponseDto>(permission);
        }
        public async Task<bool> CreateAsync(PermissionCreateDto permissionCreateDto)
        {
            var permission = await _unitOfWork.Permissions.GetFirstOrDefaultAsync(
                p => p.Key == permissionCreateDto.Key && p.Value == permissionCreateDto.Value);

            if (permission != null)
                throw new StatusCodeException(HttpStatusCode.Conflict, "Claim already exist");

            permission = (Permission)permissionCreateDto;

            await _unitOfWork.Permissions.AddAsync(permission);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(Guid permissionId)
        {
            var permission = await _unitOfWork.Permissions.GetFirstOrDefaultAsync(permissionId)
               ?? throw new StatusCodeException(HttpStatusCode.NotFound, "Not found");

            _unitOfWork.Permissions.Remove(permission);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<ICollection<PermissionResponseDto>?> GetAllAsync()
        {
            var permission = await _unitOfWork.Permissions.GetAllAsync();

            if (permission != null)
                return _mapper.Map<ICollection<PermissionResponseDto>>(permission);

            return null;
        }
        public async Task<ICollection<PermissionResponseDto>?> GetWhereAsync(Expression<Func<Permission, bool>> predicate)
        {
            var permission = await _unitOfWork.Permissions.GetWhereAsync(predicate);

            if (permission != null)
                return _mapper.Map<ICollection<PermissionResponseDto>>(permission);

            return null;
        }
    }
}
