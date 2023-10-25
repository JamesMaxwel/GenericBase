using AutoMapper;
using GenericBase.Application.Dto;
using GenericBase.Application.Dto.AccountDto;
using GenericBase.Application.Dto.Common;
using GenericBase.Application.Helpers.Exceptions;
using GenericBase.Application.Interfaces;
using GenericBase.Application.Interfaces.Common;
using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.Interfaces.Common;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace GenericBase.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailService _emailService;

        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IMapper mapper, IMemoryCache memoryCache, IEmailService emailService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _emailService = emailService;

            _unitOfWork = unitOfWork;
        }

        public async Task<AccountResponseDto?> GetByIdAsync(Guid userId)
        {
            var account = await _unitOfWork.Users.GetFirstOrDefaultAsync(userId)
               ?? throw new StatusCodeException(HttpStatusCode.NotFound, "Operator not found!");

            return _mapper.Map<AccountResponseDto>(account);
        }
        public async Task<bool> CreateAsync(AccountCreateDto accountCreateDto)
        {
            var account = await _unitOfWork.Users.GetFirstOrDefaultAsync(user => user.Email == accountCreateDto.Email);

            if (account != null)
                throw new StatusCodeException(HttpStatusCode.Conflict, "Email already exist");

            var user = (User)accountCreateDto;

            await _unitOfWork.Users.AddAsync(user);

            return await _unitOfWork.SaveChangesAsync() > 0;

        }
        public async Task<bool> UpdateAsync(Guid UserId, AccountUpdateDto accountUpdateDto)
        {
            var account = await _unitOfWork.Users.GetFirstOrDefaultAsync(UserId)
                ?? throw new StatusCodeException(HttpStatusCode.NotFound, "Operator not found!");

            account = _mapper.Map(accountUpdateDto, account);

            _unitOfWork.Users.Update(account);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<bool> ActivateAsync(Guid userId, AccountActivateDto accountActivateDto)
        {
            var account = await _unitOfWork.Users.GetFirstOrDefaultAsync(userId)
             ?? throw new StatusCodeException(HttpStatusCode.NotFound, "Operator not found!");

            account.IsEnabled = true;

            _unitOfWork.Users.Update(account);

            return await _unitOfWork.SaveChangesAsync() > 0;

        }
        public async Task<bool> SendCodeToEmailAsync(EmailAdressDto emailAdressDto)
        {
            var email = emailAdressDto.Email;
            int code = new Random().Next(100000, 999999);

            var lastCode = _memoryCache.Get(email);

            if (lastCode != null)
                code = (int)lastCode;

            _memoryCache.Set(email, code, TimeSpan.FromMinutes(10));

            var message = new EmailMessageDto()
            {
                To = email,
                Subject = "Verification code",
                Body = code.ToString()
            };

            return await _emailService.SendAsync(message);
        }

        public async Task<bool> ResetPasswordAsync(AccountResetPasswordDto accountResetPasswordDto)
        {
            int resetCode = accountResetPasswordDto.ResetCode;

            var storedCode = _memoryCache.Get($"reset{resetCode}")
                ?? throw new StatusCodeException(HttpStatusCode.BadRequest, "expired code");

            if (accountResetPasswordDto.Email != storedCode.ToString())
                throw new StatusCodeException(HttpStatusCode.BadRequest, "invalid data");

            var account = await _unitOfWork.Users.GetFirstOrDefaultAsync(user => user.Email == storedCode.ToString())
            ?? throw new StatusCodeException(HttpStatusCode.BadRequest, "invalid data");

            account.SetPassword(accountResetPasswordDto.NewPassword);

            _unitOfWork.Users.Update(account);

            return await _unitOfWork.SaveChangesAsync() > 0;

        }
        public async Task<AccountCredentialResponseDto?> GetCredentialsAsync(Guid userId)
        {
            var account = await _unitOfWork.Users.GetFirstOrDefaultWithCredentialsAsync(userId)
             ?? throw new StatusCodeException(HttpStatusCode.NotFound, "Operator not found!");

            return _mapper.Map<AccountCredentialResponseDto>(account);

        }
        public async Task<bool> UpdateCredentialsAsync(Guid userId, AccountCredentialUpdateDto accountCredentialsUpdateDto)
        {

            var account = await _unitOfWork.Users.GetFirstOrDefaultAsync(userId)
                ?? throw new StatusCodeException(HttpStatusCode.NotFound, "Operator not found!");

            account.Roles.Clear();
            account.Permissions.Clear();

            var roles = accountCredentialsUpdateDto.RolesIds.Any()
               ? await _unitOfWork.Roles.GetWhereAsync(rl => accountCredentialsUpdateDto.RolesIds.Contains(rl.Id))
               : null;

            var permissions = accountCredentialsUpdateDto.ClaimsIds.Any()
             ? await _unitOfWork.Permissions.GetWhereAsync(cl => accountCredentialsUpdateDto.ClaimsIds.Contains(cl.Id))
             : null;

            if (roles != null)
            {
                bool allIdsPresents = accountCredentialsUpdateDto.RolesIds.All(id => roles.Any(obj => obj.Id == id));

                if (!allIdsPresents)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, "One or more id was not found");

                if (roles.Count() != accountCredentialsUpdateDto.RolesIds.Count())
                    throw new StatusCodeException(HttpStatusCode.UnprocessableEntity, "Error in process Roles Ids");

                account.Roles = roles.ToList();
            }

            if (permissions != null)
            {
                bool todosOsIdsPresentes = accountCredentialsUpdateDto.ClaimsIds.All(id => permissions.Any(obj => obj.Id == id));

                if (!todosOsIdsPresentes)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, "One or more id was not found");

                if (permissions.Count() != accountCredentialsUpdateDto.ClaimsIds.Count())
                    throw new StatusCodeException(HttpStatusCode.UnprocessableEntity, "Error in process Claims Ids");

                account.Permissions = permissions.ToList();
            }

            _unitOfWork.Users.Update(account);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateEmailAsync(Guid userId, AccountEmailUpdateDto accountEmailUpdateDto)
        {
            var account = await _unitOfWork.Users.GetFirstOrDefaultAsync(userId)
               ?? throw new StatusCodeException(HttpStatusCode.NotFound, "Operator not found!");

            if (!account.IsSamePassword(accountEmailUpdateDto.Password))
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid password.");

            if (account.Email != accountEmailUpdateDto.CurrentEmail)
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid current Email");

            var anotherAccount = await _unitOfWork.Users.GetFirstOrDefaultAsync(
                user => user.Email == accountEmailUpdateDto.NewEmail && user.Id != userId);

            if (anotherAccount != null)
                throw new StatusCodeException(HttpStatusCode.Conflict, "The new email is already in use by another account");

            account.Email = accountEmailUpdateDto.NewEmail;

            _unitOfWork.Users.Update(account);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdatePasswordAsync(Guid userId, AccountPasswordUpdateDto accountPasswordUpdateDto)
        {
            var account = await _unitOfWork.Users.GetFirstOrDefaultAsync(userId)
               ?? throw new StatusCodeException(HttpStatusCode.NotFound, "Operator not found!");

            if (!account.IsSamePassword(accountPasswordUpdateDto.CurrentPassword))
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid password.");

            account.SetPassword(accountPasswordUpdateDto.NewPassword);

            _unitOfWork.Users.Update(account);

            return await _unitOfWork.SaveChangesAsync() > 0;

        }
    }
}
