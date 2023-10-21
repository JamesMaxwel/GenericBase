using AutoMapper;
using GenericBase.Application.Dto.AccountDto;
using GenericBase.Application.Dto.PermissionDto;
using GenericBase.Application.Dto.RoleDto;
using GenericBase.Domain.Entities.Account;

namespace GenericBase.Application.Helpers.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<User, AccountResponseDto>();
            CreateMap<User, AccountCredentialResponseDto>();
            CreateMap<Role, RoleResponseDto>();
            CreateMap<Permission, PermissionResponseDto>();

        }
    }
}
