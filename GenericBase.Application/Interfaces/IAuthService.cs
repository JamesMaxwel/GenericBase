using GenericBase.Application.Dto;
using GenericBase.Application.Dto.AccountDto;

namespace GenericBase.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto> GetTokenAsync(AccountLoginDto loginDto);
        Task<TokenResponseDto> GetRefreshTokenAsync(string refreshToken);

    }
}
