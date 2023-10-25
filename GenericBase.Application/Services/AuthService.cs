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
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;

        private readonly JwtSettings _jwtOptions;

        public AuthService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtOptions)
        {
            _unitOfWork = unitOfWork;
            _jwtOptions = jwtOptions.Value;

            _userRepository = unitOfWork.Users;
            _roleRepository = unitOfWork.Roles;
            _permissionRepository = unitOfWork.Permissions;
        }

        public async Task<TokenResponseDto> GetTokenAsync(AccountLoginDto login)
        {

            var account = await _userRepository.GetFirstOrDefaultAsync(user => user.Email == login.Email)
                ?? throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid username or password");

            if (account.IsLockedOut)
                throw new StatusCodeException(HttpStatusCode.Forbidden, "Account blocked");

            if (!account.IsSamePassword(login.Password))
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid username or password.");

            var token = await CreateTokenAsync(account.Id);

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

            var tkJti = result.Claims.FirstOrDefault(
                cl => cl.Key == JwtRegisteredClaimNames.Jti).Value?.ToString()
                 ?? throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid token");

            if (!account.Permissions.Any(c => c.Key == "last-refresh-token" && c.Value == tkJti))
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Expired token");

            if (account.IsLockedOut)
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Operator blocked");

            var newToken = await CreateTokenAsync(account.Id);

            return newToken;
        }

        private async Task<TokenResponseDto> CreateTokenAsync(Guid userId)
        {
            var user = await _userRepository.GetFirstOrDefaultWithCredentialsAsync(userId)
                ?? throw new StatusCodeException(HttpStatusCode.InternalServerError, "Error on generated token");

            var at = GenerateAccessToken(user)
               ?? throw new StatusCodeException(HttpStatusCode.InternalServerError, "Error on generated 'at' token");

            var jti = Guid.NewGuid().ToString();

            var rt = GenerateRefreshToken(user, jti)
                ?? throw new StatusCodeException(HttpStatusCode.InternalServerError, "Error on generated 'rt' token");

            var lastRefreshTokenClaim = user.Permissions.FirstOrDefault(cl => cl.Key == "LastRefreshToken");

            if (lastRefreshTokenClaim == null)
            {
                var newClaim = new Permission("LastRefreshToken", jti, false);
                await _permissionRepository.AddAsync(newClaim);
                user.Permissions.Add(newClaim);
            }
            else
            {
                lastRefreshTokenClaim.Value = jti;
            }

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return new TokenResponseDto(at, rt, _jwtOptions.AccessTokenExpiration, _jwtOptions.RefreshTokenExpiration);
        }
        private string GenerateAccessToken(User user)
        {

            var claims = new ClaimsIdentity();

            user.Roles.ToList().ForEach(role => role.Permissions.ToList()
            .ForEach(p => claims.AddClaim(new Claim(p.Key, p.Value))));

            user.Permissions.Where(p => p.IsPublicInToken == true).ToList()
                .ForEach(cl => claims.AddClaim(new Claim(cl.Key, cl.Value)));

            claims.AddClaim(new Claim("sub", user.Id.ToString()));
            claims.AddClaim(new Claim("jti", Guid.NewGuid().ToString()));

            var handler = new JwtSecurityTokenHandler();

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
            claims.AddClaim(new Claim("sub", user.Id.ToString()));
            claims.AddClaim(new Claim("jti", jti));

            var handler = new JwtSecurityTokenHandler();

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = _jwtOptions.SigningCredentials,
                Subject = claims,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(_jwtOptions.RefreshTokenExpiration),
                TokenType = "rt+jwt"
            });

            var encodedJwt = handler.WriteToken(securityToken);
            return encodedJwt;
        }
    }
}
