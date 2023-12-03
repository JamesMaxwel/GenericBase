using GenericBase.Application.Dto.AccountDto;
using GenericBase.Application.Dto.Common;

namespace GenericBase.Application.Interfaces
{
    public interface IAccountService
    {
        Task<AccountCredentialResponseDto?> GetCredentialsAsync(Guid userId);
        Task<AccountResponseDto?> GetByIdAsync(Guid userId);
        Task<bool> ActivateAsync(Guid userId, AccountActivateDto accountActivateDto);
        Task<AccountResponseDto?> CreateAsync(AccountCreateDto accountCreateDto);
        Task<bool> ResetPasswordAsync(AccountResetPasswordDto accountResetPasswordDto);
        Task<bool> SendCodeToEmailAsync(EmailAdressDto emailAdressDto);
        Task<bool> UpdateAsync(Guid userId, AccountUpdateDto accountUpdateDto);
        Task<bool> UpdateCredentialsAsync(Guid userId, AccountCredentialUpdateDto accountUpdateCredentialsDto);
        Task<bool> UpdateEmailAsync(Guid userId, AccountEmailUpdateDto accountEmailUpdateDto);
        Task<bool> UpdatePasswordAsync(Guid userId, AccountPasswordUpdateDto accountPasswordUpdateDto);
    }
}
