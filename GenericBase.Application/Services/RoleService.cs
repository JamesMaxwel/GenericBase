using AutoMapper;
using GenericBase.Application.Dto.RoleDto;
using GenericBase.Application.Helpers.Exceptions;
using GenericBase.Application.Interfaces;
using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.Interfaces.Common;
using System.Linq.Expressions;
using System.Net;

namespace GenericBase.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<RoleResponseDto> GetByIdAsync(Guid roleId)
        {
            var role = await _unitOfWork.Roles.GetFirstOrDefaultAsync(roleId)
              ?? throw new StatusCodeException(HttpStatusCode.NotFound, "Not found");

            return _mapper.Map<RoleResponseDto>(role);
        }
        public async Task<bool> CreateAsync(RoleCreateDto roleCreateDto)
        {
            var role = await _unitOfWork.Roles.GetFirstOrDefaultAsync(r => r.Name == roleCreateDto.Name);
            if (role != null)
                throw new StatusCodeException(HttpStatusCode.Conflict, "Manager already exist");

            role = (Role)roleCreateDto;

            var permissions = roleCreateDto.ClaimsIds.Any()
               ? await _unitOfWork.Permissions.GetWhereAsync(cl => roleCreateDto.ClaimsIds.Contains(cl.Id))
               : null;

            if (permissions != null)
            {
                bool todosOsIdsPresentes = roleCreateDto.ClaimsIds.All(id => permissions.Any(objeto => objeto.Id == id));

                if (!todosOsIdsPresentes)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, "One or more id was not found");

                if (permissions.Count() != roleCreateDto.ClaimsIds.Count)
                    throw new StatusCodeException(HttpStatusCode.UnprocessableEntity, "Error in process Claims Ids");

                role.Permissions = permissions.ToList();
            }

            await _unitOfWork.Roles.AddAsync(role);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateAsync(Guid roleId, RoleUpdateDto roleUpdateDto)
        {
            if (roleUpdateDto.Id != roleId)
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Not match Ids");

            var role = await _unitOfWork.Roles.GetFirstOrDefaultAsync(roleId)
               ?? throw new StatusCodeException(HttpStatusCode.NotFound, "Not found");

            if (role.Name != roleUpdateDto.Name)
            {
                var anotherRole = await _unitOfWork.Roles.GetFirstOrDefaultAsync(r => r.Name == roleUpdateDto.Name && r.Id != roleId);

                if (anotherRole != null)
                    throw new StatusCodeException(HttpStatusCode.Conflict, "There is already another ROLE for the new name");

                role.Name = roleUpdateDto.Name;
            }

            var permissions = roleUpdateDto.ClaimsIds.Any()
                ? await _unitOfWork.Permissions.GetWhereAsync(p => roleUpdateDto.ClaimsIds.Contains(p.Id))
                : null;

            role.Permissions.Clear();

            if (permissions != null)
            {
                bool todosOsIdsPresentes = roleUpdateDto.ClaimsIds.All(id => permissions.Any(objeto => objeto.Id == id));

                if (!todosOsIdsPresentes)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, "One or more id was not found");

                if (permissions.Count() != roleUpdateDto.ClaimsIds.Count)
                    throw new StatusCodeException(HttpStatusCode.UnprocessableEntity, "Error in process Claims Ids");

                role.Permissions = permissions.ToList();
            }

            _unitOfWork.Roles.Update(role);

            return await _unitOfWork.SaveChangesAsync() > 0;

        }
        public async Task<bool> DeleteAsync(Guid roleId)
        {
            var role = await _unitOfWork.Roles.GetFirstOrDefaultAsync(roleId)
                ?? throw new StatusCodeException(HttpStatusCode.NotFound, "Not found");

            _unitOfWork.Roles.Remove(role);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<ICollection<RoleResponseDto>?> GetWhereAsync(Expression<Func<Role, bool>> predicate)
        {
            var roles = await _unitOfWork.Roles.GetWhereAsync(predicate);

            if (roles != null)
                return _mapper.Map<ICollection<RoleResponseDto>>(roles);

            return null;
        }
        public async Task<ICollection<RoleResponseDto>?> GetAllAsync()
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();

            if (roles != null)
                return _mapper.Map<ICollection<RoleResponseDto>>(roles);

            return null;
        }
    }
}
