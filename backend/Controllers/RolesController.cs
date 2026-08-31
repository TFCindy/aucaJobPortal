using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;

namespace AucaJobPortalDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly IPermissionService _permissionService;

    public RolesController(IRoleService roleService, IPermissionService permissionService)
    {
        _roleService = roleService;
        _permissionService = permissionService;
    }

    private int GetRoleId()
    {
        var claim = User.FindFirst("roleId")?.Value;
        if (string.IsNullOrEmpty(claim)) throw new Exception("Role ID not found in token");
        return int.Parse(claim);
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "manage_roles"))
            {
                return Forbid();
            }

            var roles = await _roleService.GetRolesAsync();
            return Ok(new { success = true, data = roles });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById(int id)
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "manage_roles"))
            {
                return Forbid();
            }

            var role = await _roleService.GetRoleByIdAsync(id);
            return Ok(new { success = true, data = role });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("permissions")]
    public async Task<IActionResult> GetPermissions()
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "manage_roles"))
            {
                return Forbid();
            }

            var permissions = await _roleService.GetPermissionsAsync();
            return Ok(new { success = true, data = permissions });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPut("{id}/permissions")]
public async Task<IActionResult> UpdateRolePermissions(int id, [FromBody] UpdateRolePermissionsDTO dto)
{
    try
    {
        if (!await _permissionService.HasPermissionAsync(GetRoleId(), "manage_roles"))
        {
            return Forbid();
        }

        var result = await _roleService.UpdateRolePermissionsAsync(id, dto?.Permissions ?? new List<string>());
        if (!result)
            return NotFound(new { success = false, message = "Role not found" });

        return Ok(new { success = true, message = "Permissions updated successfully" });
    }
    catch (Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}
}