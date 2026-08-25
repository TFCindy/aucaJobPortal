using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AucaJobPortalDotNet.Interfaces;

namespace AucaJobPortalDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("dashboard-stats")]
    public async Task<IActionResult> GetDashboardStats()
    {
        try
        {
            var stats = await _adminService.GetDashboardStatsAsync();
            return Ok(new { success = true, data = stats });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}