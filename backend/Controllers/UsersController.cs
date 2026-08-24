// Controllers/UsersController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;

namespace AucaJobPortalDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPermissionService _permissionService;

    public UsersController(IUserService userService, IPermissionService permissionService)
    {
        _userService = userService;
        _permissionService = permissionService;
    }

    private int GetRoleId()
    {
        var claim = User.FindFirst("roleId")?.Value;
        if (string.IsNullOrEmpty(claim)) throw new Exception("Role ID not found in token");
        return int.Parse(claim);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "manage_users"))
            {
                return Forbid();
            }

            var users = await _userService.GetUsersAsync();
            return Ok(new { success = true, data = users, count = users.Count });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> DeactivateUser(int id)
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "manage_users"))
            {
                return Forbid();
            }

            var result = await _userService.DeactivateUserAsync(id);
            if (!result)
                return NotFound(new { success = false, message = "User not found" });

            return Ok(new { success = true, message = "User deactivated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> ActivateUser(int id)
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "manage_users"))
            {
                return Forbid();
            }

            var result = await _userService.ActivateUserAsync(id);
            if (!result)
                return NotFound(new { success = false, message = "User not found" });

            return Ok(new { success = true, message = "User activated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}