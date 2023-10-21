using GenericBase.Application.Dto;

namespace GenericBase.Application.Interfaces.Common
{
    public interface IEmailService
    {
        Task<bool> SendAsync(EmailMessageDto emailMessage);
    }
}
