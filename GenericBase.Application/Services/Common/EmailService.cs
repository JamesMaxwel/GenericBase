using GenericBase.Application.Dto;
using GenericBase.Application.Helpers.Options;
using GenericBase.Application.Interfaces.Common;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace GenericBase.Application.Services.Common
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task<bool> SendAsync(EmailMessageDto emailMessage)
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(_emailSettings.EmailAddress));
            mail.To.Add(MailboxAddress.Parse(emailMessage.To));
            mail.Subject = emailMessage.Subject;
            mail.Body = new TextPart(TextFormat.Html) { Text = emailMessage.Body };

            var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.EmailAddress, _emailSettings.Password);
            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);

            return true;
        }
    }
}
