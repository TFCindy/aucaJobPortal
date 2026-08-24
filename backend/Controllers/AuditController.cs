using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;

namespace AucaJobPortalDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditController : ControllerBase
{
    private readonly IAuditService _auditService;
    private readonly IPermissionService _permissionService;

    public AuditController(IAuditService auditService, IPermissionService permissionService)
    {
        _auditService = auditService;
        _permissionService = permissionService;
    }

    private int GetRoleId()
    {
        var claim = User.FindFirst("roleId")?.Value;
        if (string.IsNullOrEmpty(claim)) throw new Exception("Role ID not found in token");
        return int.Parse(claim);
    }

    [HttpGet("logs")]
    public async Task<IActionResult> GetAuditLogs(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] string? action,
        [FromQuery] string? entityType,
        [FromQuery] bool? isSuccessful,
        [FromQuery] int? limit)
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "view_audit_logs"))
            {
                return Forbid();
            }

            var logs = await _auditService.GetAuditLogsAsync(fromDate, toDate, action, entityType, isSuccessful, limit);
            
            return Ok(new
            {
                success = true,
                data = logs,
                count = logs.Count
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("logs/{id}")]
    public async Task<IActionResult> GetAuditLogById(int id)
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "view_audit_logs"))
            {
                return Forbid();
            }

            var log = await _auditService.GetAuditLogByIdAsync(id);
            return Ok(new { success = true, data = log });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("actions")]
    public async Task<IActionResult> GetActionTypes()
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "view_audit_logs"))
            {
                return Forbid();
            }

            var actions = await _auditService.GetActionTypesAsync();
            return Ok(new
            {
                success = true,
                data = actions
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}