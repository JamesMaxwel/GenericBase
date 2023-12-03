using GenericBase.Application.Dto;
using GenericBase.Application.Dto.AccountDto;
using GenericBase.Application.Helpers.Exceptions;
using GenericBase.Application.Helpers.Options;
using GenericBase.Application.Interfaces;
using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.Interfaces.Account;
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
        private readonly IUserRepository _userRepository;


        private readonly JwtSettings _jwtOptions;

        public AuthService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtOptions)
        {
            _unitOfWork = unitOfWork;
            _jwtOptions = jwtOptions.Value;
            _userRepository = unitOfWork.Users;
        }
        public async Task<TokenResponseDto> GetTokenAsync(AccountLoginDto login)
        {

            var account = await _userRepository.GetFirstOrDefaultWithCredentialsAsync(user => user.Email == login.Email)
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
            var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
            var result = handler.ValidateToken(refreshToken, new TokenValidationParameters()
            {
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                RequireSignedTokens = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SecretKey)),
            });

            if (!result.IsValid)
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Expired token");

            var tkSub = result.Claims.FirstOrDefault(
                cl => cl.Key == JwtRegisteredClaimNames.Sub).Value?.ToString()
                ?? throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid token");

            if (!Guid.TryParse(tkSub.ToString(), out Guid tkUserId))
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid token");

            var account = await _userRepository.GetFirstOrDefaultWithCredentialsAsync(tkUserId)
              ?? throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid token");


            if (!result.Claims.Any(c => c.Key == JwtRegisteredClaimNames.Jti && c.Value?.ToString() == account.LastJti))
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid token");

            if (account.IsLockedOut)
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Account blocked");

            var newToken = await CreateTokenAsync(account);

            return newToken;
        }
        private async Task<TokenResponseDto> CreateTokenAsync(User user)
        {

            var at = GenerateAccessToken(user)
               ?? throw new StatusCodeException(HttpStatusCode.InternalServerError, "Error on generated 'at' token");

            var jti = Guid.NewGuid().ToString();

            var rt = GenerateRefreshToken(user, jti)
                ?? throw new StatusCodeException(HttpStatusCode.InternalServerError, "Error on generated 'rt' token");

            user.LastJti = jti;

            _userRepository.Update(user);
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
