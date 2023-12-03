using GenericBase.Application.Dto;
using GenericBase.Application.Dto.AccountDto;
using GenericBase.Application.Helpers;
using GenericBase.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GenericBase.API.Controllers
{
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        [SwaggerOperation(Summary = "Get access token", Description = "Retrieves the access token using the provided credentials.")]
        [SwaggerResponse(200, "Returns a new access token upon successful authentication.", typeof(TokenResponseDto))]
        [SwaggerResponse(204, "If the authentication fails.")]
        public async Task<ActionResult<TokenResponseDto>> GetToken([FromBody] AccountLoginDto login)
        {
            var result = await _authService.GetTokenAsync(login);

            return result != null
                ? Ok(result)
                : NoContent();

        }

        [HttpPost("refresh-token")]
        [SwaggerOperation(Summary = "Refresh access token", Description = "Refreshes the access token using a valid refresh token.")]
        [SwaggerResponse(200, "Returns a new access token upon successful refresh.", typeof(TokenResponseDto))]
        [SwaggerResponse(204, "If the refresh token is not valid or expired.")]
        [SwaggerResponse(400, "If the refresh token is not provided.")]
        public async Task<ActionResult<TokenResponseDto>> GetRefreshToken()
        {
            var token = HttpContextHelpers.GetAccessToken(HttpContext);

            if (token is null)
                return BadRequest("Refresh token is required");

            var result = await _authService.GetRefreshTokenAsync(token);

            return result != null
                   ? Ok(result)
                   : NoContent();
        }
    }
}
