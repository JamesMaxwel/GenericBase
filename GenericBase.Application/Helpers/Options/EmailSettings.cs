using Microsoft.Extensions.Configuration;

namespace GenericBase.Application.Helpers.Options
{
    public class EmailSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public static EmailSettings FromConfiguration(IConfiguration configuration)
        {
            var emailOptions = configuration.GetSection(nameof(EmailSettings));

            var host = emailOptions.GetSection(nameof(Host)).Value;
            var port = int.Parse(emailOptions.GetSection(nameof(Port)).Value);
            var email = emailOptions.GetSection(nameof(EmailAddress)).Value;
            var password = emailOptions.GetSection(nameof(Password)).Value;

            return new EmailSettings
            {
                Host = host,
                Port = port,
                EmailAddress = email,
                Password = password
            };
        }
    }
}
