using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;

namespace AucaJobPortalDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CounselorsController : ControllerBase
{
    private readonly ICounselorService _counselorService;
    private readonly IPermissionService _permissionService;

    public CounselorsController(ICounselorService counselorService, IPermissionService permissionService)
    {
        _counselorService = counselorService;
        _permissionService = permissionService;
    }

    private int GetRoleId()
    {
        var claim = User.FindFirst("roleId")?.Value;
        if (string.IsNullOrEmpty(claim)) throw new Exception("Role ID not found in token");
        return int.Parse(claim);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCounselor([FromBody] CreateCounselorDTO dto)
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "manage_counselors"))
            {
                return Forbid();
            }

            var counselor = await _counselorService.CreateCounselorAsync(dto);
            return Ok(new
            {
                success = true,
                message = "Career counselor created successfully",
                data = counselor
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetCounselors()
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "view_counselors"))
            {
                return Forbid();
            }

            var counselors = await _counselorService.GetAllCounselorsAsync();
            return Ok(new
            {
                success = true,
                data = counselors,
                count = counselors.Count
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCounselorById(int id)
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "view_counselors"))
            {
                return Forbid();
            }

            var counselor = await _counselorService.GetCounselorByIdAsync(id);
            return Ok(new { success = true, data = counselor });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> DeactivateCounselor(int id)
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "manage_counselors"))
            {
                return Forbid();
            }

            var result = await _counselorService.DeactivateCounselorAsync(id);
            if (!result)
                return NotFound(new { success = false, message = "Counselor not found" });

            return Ok(new
            {
                success = true,
                message = "Counselor deactivated successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> ActivateCounselor(int id)
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "manage_counselors"))
            {
                return Forbid();
            }

            var result = await _counselorService.ActivateCounselorAsync(id);
            if (!result)
                return NotFound(new { success = false, message = "Counselor not found" });

            return Ok(new
            {
                success = true,
                message = "Counselor activated successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}