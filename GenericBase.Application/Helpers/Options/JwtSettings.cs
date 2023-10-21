using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GenericBase.Application.Helpers.Options;

public class JwtSettings
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int AccessTokenExpiration { get; set; }
    public int RefreshTokenExpiration { get; set; }

    public SigningCredentials? SigningCredentials { get; set; }
    public SymmetricSecurityKey? SymmetricSecurityKey { get; set; }

    public static JwtSettings FromConfiguration(IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtSettings));

        var issuer = jwtOptions.GetSection(nameof(Issuer)).Value;
        var audience = jwtOptions.GetSection(nameof(Audience)).Value;
        var secretKey = jwtOptions.GetSection(nameof(SecretKey)).Value;

        if (!int.TryParse(jwtOptions.GetSection(nameof(AccessTokenExpiration)).Value, out int accessTokenExpiration))
            accessTokenExpiration = 15;

        if (!int.TryParse(jwtOptions.GetSection(nameof(RefreshTokenExpiration)).Value, out int refreshTokenExpiration))
            refreshTokenExpiration = 60 * 24 * 7;

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

        return new JwtSettings
        {
            Issuer = issuer,
            Audience = audience,
            SecretKey = secretKey,
            AccessTokenExpiration = accessTokenExpiration,
            RefreshTokenExpiration = refreshTokenExpiration,
            SymmetricSecurityKey = symmetricSecurityKey,
            SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256),
        };
    }
}