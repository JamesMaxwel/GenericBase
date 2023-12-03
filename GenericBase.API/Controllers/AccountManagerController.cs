using GenericBase.Application.Dto.AccountDto;
using GenericBase.Application.Dto.PermissionDto;
using GenericBase.Application.Dto.RoleDto;
using GenericBase.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GenericBase.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountManagerController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;

        public AccountManagerController(IAccountService accountService, IRoleService roleService, IPermissionService permissionService)
        {
            _accountService = accountService;
            _roleService = roleService;
            _permissionService = permissionService;
        }

        [HttpGet("profile/{userId}")]
        [SwaggerOperation(Summary = "Get user profile by ID", Description = "Retrieves the user profile by user ID.")]
        [SwaggerResponse(200, "Returns the user profile.", typeof(AccountResponseDto))]
        [SwaggerResponse(204, "If the user profile is not found.")]
        public async Task<ActionResult<AccountResponseDto>> GetProfile(Guid userId)
        {
            var result = await _accountService.GetByIdAsync(userId);

            return result != null
               ? Ok(result)
               : NoContent();
        }

        [HttpPut("profile/{userId}")]
        [SwaggerOperation(Summary = "Update user profile", Description = "Updates the profile of a user.")]
        [SwaggerResponse(200, "Profile update successful.")]
        [SwaggerResponse(422, "If profile update fails.")]
        public async Task<IActionResult> UpdateProfile(Guid userId, [FromBody] AccountUpdateDto accountUpdateDto)
        {
            var result = await _accountService.UpdateAsync(userId, accountUpdateDto);

            return result
               ? Ok()
               : UnprocessableEntity();
        }

        [HttpGet("credentials/{userId}")]
        [SwaggerOperation(Summary = "Get user credentials by ID", Description = "Retrieves the credentials of a user by user ID.")]
        [SwaggerResponse(200, "Returns the user credentials.", typeof(AccountCredentialResponseDto))]
        [SwaggerResponse(204, "If the user credentials are not found.")]
        public async Task<ActionResult<AccountCredentialResponseDto>> GetCredentials(Guid userId)
        {
            var result = await _accountService.GetCredentialsAsync(userId);

            return result != null
               ? Ok(result)
               : NoContent();
        }

        [HttpPut("credentials/{userId}")]
        [SwaggerOperation(Summary = "Update user credentials", Description = "Updates the credentials of a user.")]
        [SwaggerResponse(200, "Credentials update successful.")]
        [SwaggerResponse(422, "If credentials update fails.")]
        public async Task<IActionResult> UpdateCredentials(Guid userId, [FromBody] AccountCredentialUpdateDto accountUpdateCredentialsDto)
        {
            var result = await _accountService.UpdateCredentialsAsync(userId, accountUpdateCredentialsDto);

            return result
               ? Ok()
               : UnprocessableEntity();
        }

        [HttpGet("role")]
        [SwaggerOperation(Summary = "Get roles", Description = "Retrieves a list of roles.")]
        [SwaggerResponse(200, "Returns a list of roles.", typeof(List<RoleResponseDto>))]
        [SwaggerResponse(204, "If no roles are found.")]
        public async Task<ActionResult<ICollection<RoleResponseDto>>> GetRoles(string? name)
        {
            var result = (name == null)
                ? await _roleService.GetAllAsync()
                : await _roleService.GetWhereAsync(r => r.Name == name);

            return result != null && result.Any()
                ? Ok(result)
                : NoContent();
        }

        [HttpPost("role")]
        [SwaggerOperation(Summary = "Create role", Description = "Creates a new role.")]
        [SwaggerResponse(200, "Role creation successful.")]
        [SwaggerResponse(422, "If role creation fails.")]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto roleCreateDto)
        {
            var result = await _roleService.CreateAsync(roleCreateDto);

            return result
            ? Ok()
            : UnprocessableEntity();
        }

        [HttpGet("role/{roleId}")]
        [SwaggerOperation(Summary = "Get role by ID", Description = "Retrieves a role by role ID.")]
        [SwaggerResponse(200, "Returns the role.", typeof(RoleResponseDto))]
        [SwaggerResponse(204, "If the role is not found.")]
        public async Task<ActionResult<RoleResponseDto>> GetRoleById(Guid roleId)
        {
            var result = await _roleService.GetByIdAsync(roleId);

            return result != null
                ? Ok(result)
                : NoContent();
        }

        [HttpPut("role/{roleId}")]
        [SwaggerOperation(Summary = "Update role by ID", Description = "Updates a role by role ID.")]
        [SwaggerResponse(200, "Role update successful.")]
        [SwaggerResponse(422, "If role update fails.")]
        public async Task<ActionResult<RoleResponseDto>> Update(Guid roleId, RoleUpdateDto roleUpdateDto)
        {
            var result = await _roleService.UpdateAsync(roleId, roleUpdateDto);

            return result
            ? Ok()
            : UnprocessableEntity();
        }

        [HttpDelete("role/{roleId}")]
        [SwaggerOperation(Summary = "Delete role by ID", Description = "Deletes a role by role ID.")]
        [SwaggerResponse(200, "Role deletion successful.")]
        [SwaggerResponse(422, "If role deletion fails.")]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            var result = await _roleService.DeleteAsync(roleId);

            return result
            ? Ok()
            : UnprocessableEntity();
        }

        [HttpGet("claim")]
        [SwaggerOperation(Summary = "Get claims", Description = "Retrieves a list of claims.")]
        [SwaggerResponse(200, "Returns a list of claims.", typeof(List<PermissionResponseDto>))]
        [SwaggerResponse(204, "If no claims are found.")]
        public async Task<ActionResult<ICollection<PermissionResponseDto>>> GetClaims(string? key)
        {
            var result = (key is null)
                ? await _permissionService.GetAllAsync()
                : await _permissionService.GetWhereAsync(c => c.Key == key);

            return result != null && result.Any()
            ? Ok(result)
            : NoContent();
        }

        [HttpPost("claim")]
        [SwaggerOperation(Summary = "Create claim", Description = "Creates a new claim.")]
        [SwaggerResponse(200, "Claim creation successful.")]
        [SwaggerResponse(422, "If claim creation fails.")]
        public async Task<IActionResult> CreateClaim([FromBody] PermissionCreateDto permissionCreateDto)
        {
            var result = await _permissionService.CreateAsync(permissionCreateDto);

            return result
            ? Ok()
            : UnprocessableEntity();
        }

        [HttpGet("claim/{claimId}")]
        [SwaggerOperation(Summary = "Get claim by ID", Description = "Retrieves a claim by claim ID.")]
        [SwaggerResponse(200, "Returns the claim.", typeof(PermissionResponseDto))]
        [SwaggerResponse(204, "If the claim is not found.")]
        public async Task<ActionResult<PermissionResponseDto>> GetPermissionById(Guid claimId)
        {
            var result = await _permissionService.GetByIdAsync(claimId);

            return result != null
                ? Ok(result)
                : NoContent();
        }

        [HttpDelete("claim/{claimId}")]
        [SwaggerOperation(Summary = "Delete claim by ID", Description = "Deletes a claim by claim ID.")]
        [SwaggerResponse(200, "Claim deletion successful.")]
        [SwaggerResponse(422, "If claim deletion fails.")]
        public async Task<IActionResult> DeleteClaim(Guid claimId)
        {
            var result = await _permissionService.DeleteAsync(claimId);

            return result
            ? Ok()
            : UnprocessableEntity();
        }
    }
}
