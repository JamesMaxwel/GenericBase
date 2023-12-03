using GenericBase.Application.Dto.AccountDto;
using GenericBase.Application.Dto.Common;
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
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        [ActionName(nameof(Register))]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Register new account", Description = "Registers a new user account.")]
        [SwaggerResponse(201, "Returns the created account.", typeof(AccountCreateDto))]
        [SwaggerResponse(422, "If the registration fails.")]
        public async Task<IActionResult> Register([FromBody] AccountCreateDto accountRegister)
        {
            var result = await _accountService.CreateAsync(accountRegister);

            return result != null
                ? CreatedAtAction(nameof(Register), new { id = result.Id }, result)
                : UnprocessableEntity();
        }

        [HttpPost("activate")]
        [SwaggerOperation(Summary = "Activate user account", Description = "Activates a user account.")]
        [SwaggerResponse(200, "Account activation successful.")]
        [SwaggerResponse(422, "If the activation fails.")]
        public async Task<IActionResult> Activate([FromBody] AccountActivateDto accountActivateDto)
        {
            var userIdToken = HttpContextHelpers.GetUserId(HttpContext);
            if (!Guid.TryParse(userIdToken, out Guid userId))
                return BadRequest("Invalid token data");

            var result = await _accountService.ActivateAsync(userId, accountActivateDto);

            return result
                ? Ok()
                : UnprocessableEntity();
        }

        [HttpPost("get-code")]
        [SwaggerOperation(Summary = "Get code by email", Description = "Sends a verification code to the provided email address.")]
        [SwaggerResponse(200, "Code sent successfully.")]
        [SwaggerResponse(422, "If sending code fails.")]
        public async Task<IActionResult> GetCodeByEmail([FromBody] EmailAdressDto emailAddres)
        {
            var result = await _accountService.SendCodeToEmailAsync(emailAddres);

            return result
                ? Ok()
                : UnprocessableEntity();
        }

        [HttpPost("reset-password")]
        [SwaggerOperation(Summary = "Reset account password", Description = "Resets the password for a user account.")]
        [SwaggerResponse(200, "Password reset successful.")]
        [SwaggerResponse(422, "If password reset fails.")]
        public async Task<IActionResult> ResetPassword([FromBody] AccountResetPasswordDto accountResetPasswordDto)
        {
            var result = await _accountService.ResetPasswordAsync(accountResetPasswordDto);

            return result
                ? Ok()
                : UnprocessableEntity();
        }

        [HttpGet("me/profile")]
        [SwaggerOperation(Summary = "Get user profile", Description = "Retrieves the profile of the currently authenticated user.")]
        [SwaggerResponse(200, "Returns the user profile.", typeof(AccountResponseDto))]
        [SwaggerResponse(204, "If the user profile is not found.")]
        public async Task<ActionResult<AccountResponseDto>> GetMeProfile()
        {
            var userIdToken = HttpContextHelpers.GetUserId(HttpContext);
            if (!Guid.TryParse(userIdToken, out Guid userId))
                return BadRequest("Invalid token data");

            var account = await _accountService.GetByIdAsync(userId);

            return account != null
                ? Ok(account)
                : NoContent();
        }

        [HttpPut("me/profile")]
        [SwaggerOperation(Summary = "Update user profile", Description = "Updates the profile of the currently authenticated user.")]
        [SwaggerResponse(200, "Profile update successful.")]
        [SwaggerResponse(422, "If profile update fails.")]
        public async Task<IActionResult> UpdateMeProfile([FromBody] AccountUpdateDto accountUpdateDto)
        {
            var userIdToken = HttpContextHelpers.GetUserId(HttpContext);
            if (!Guid.TryParse(userIdToken, out Guid userId))
                return BadRequest("Invalid token data");

            var result = await _accountService.UpdateAsync(userId, accountUpdateDto);

            return result
                ? Ok()
                : UnprocessableEntity();
        }

        [HttpPut("me/profile/email")]
        [SwaggerOperation(Summary = "Update user email", Description = "Updates the email address of the currently authenticated user.")]
        [SwaggerResponse(200, "Email update successful.")]
        [SwaggerResponse(422, "If email update fails.")]
        public async Task<IActionResult> UpdateMeEmail([FromBody] AccountEmailUpdateDto accountEmailUpdateDto)
        {
            var userIdToken = HttpContextHelpers.GetUserId(HttpContext);
            if (!Guid.TryParse(userIdToken, out Guid userId))
                return BadRequest("Invalid token data");

            var result = await _accountService.UpdateEmailAsync(userId, accountEmailUpdateDto);

            return result
                ? Ok()
                : UnprocessableEntity();
        }

        [HttpPut("me/profile/password")]
        [SwaggerOperation(Summary = "Update user password", Description = "Updates the password of the currently authenticated user.")]
        [SwaggerResponse(200, "Password update successful.")]
        [SwaggerResponse(422, "If password update fails.")]
        public async Task<IActionResult> UpdateMePassword([FromBody] AccountPasswordUpdateDto accountPasswordUpdateDto)
        {
            var userIdToken = HttpContextHelpers.GetUserId(HttpContext);
            if (!Guid.TryParse(userIdToken, out Guid userId))
                return BadRequest("Invalid token data");

            var result = await _accountService.UpdatePasswordAsync(userId, accountPasswordUpdateDto);

            return result
                ? Ok()
                : UnprocessableEntity();
        }
    }
}
