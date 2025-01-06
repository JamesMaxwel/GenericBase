using GenericBase.Application.Dto;
using GenericBase.Application.Dto.AccountDto;
using GenericBase.Application.Helpers.Exceptions;
using GenericBase.Application.Helpers.Options;
using GenericBase.Application.Interfaces;
using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.Interfaces.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace GenericBase.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly JwtSettings _jwtOptions;

        public AuthService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtOptions)
        {
            _unitOfWork = unitOfWork;
            _jwtOptions = jwtOptions.Value;

        }
        public async Task<TokenResponseDto> GetTokenAsync(AccountLoginDto login)
        {

            var account = await _unitOfWork.Users.GetFirstOrDefaultWithCredentialsAsync(user => user.Email == login.Email)
                ?? throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid username or password");

            if (account.IsLockedOut)
                throw new StatusCodeException(HttpStatusCode.Forbidden, "Account blocked");

            if (!account.IsSamePassword(login.Password))
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid username or password.");

            var token = await CreateTokenAsync(account);

            return token;
        }
        public async Task<TokenResponseDto> GetRefreshTokenAsync(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SecretKey)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true, // Valida se o token está expirado
                ClockSkew = TimeSpan.Zero // Opcional: remove tolerância padrão para expiração
            };

            try
            {
                var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid token");
                }

                var tkSub = principal.Claims.FirstOrDefault(cl => cl.Type == JwtRegisteredClaimNames.Sub)?.Value
                    ?? throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid token");

                if (!Guid.TryParse(tkSub, out var tkUserId))
                    throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid token");

                var account = await _unitOfWork.Users.GetFirstOrDefaultWithCredentialsAsync(tkUserId)
                    ?? throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid token");

                var jtiClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
                if (jtiClaim != account.LastJti)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid token");

                if (account.IsLockedOut)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, "Account blocked");

                var newToken = await CreateTokenAsync(account);

                return newToken;
            }
            catch (SecurityTokenException)
            {
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid or expired token");
            }
            catch (Exception ex)
            {
                throw new StatusCodeException(HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task<TokenResponseDto> CreateTokenAsync(User user)
        {

            var at = GenerateAccessToken(user)
               ?? throw new StatusCodeException(HttpStatusCode.InternalServerError, "Error on generated 'at' token");

            var jti = Guid.NewGuid().ToString();

            var rt = GenerateRefreshToken(user, jti)
                ?? throw new StatusCodeException(HttpStatusCode.InternalServerError, "Error on generated 'rt' token");

            user.LastJti = jti;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return new TokenResponseDto(at, rt, _jwtOptions.AccessTokenExpiration, _jwtOptions.RefreshTokenExpiration);
        }
        private string GenerateAccessToken(User user)
        {

            var claims = new ClaimsIdentity();

            var roleClaims = user.Roles.SelectMany(role => role.Permissions.Select(p => new Claim(p.Key, p.Value)));
            var userClaims = user.Permissions.Select(cl => new Claim(cl.Key, cl.Value));

            claims.AddClaims(roleClaims);
            claims.AddClaims(userClaims);

            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var handler = new JwtSecurityTokenHandler() { MapInboundClaims = false };

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = _jwtOptions.SigningCredentials,
                Subject = claims,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiration),
                IssuedAt = DateTime.UtcNow,
                TokenType = "at+jwt"
            });

            var encodedJwt = handler.WriteToken(securityToken);
            return encodedJwt;
        }
        private string GenerateRefreshToken(User user, string jti)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, jti));

            var handler = new JwtSecurityTokenHandler() { MapInboundClaims = false };

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = _jwtOptions.SigningCredentials,
                Subject = claims,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshTokenExpiration),
                TokenType = "rt+jwt"
            });

            var encodedJwt = handler.WriteToken(securityToken);
            return encodedJwt;
        }

    }
}
