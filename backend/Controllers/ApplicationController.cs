using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Services.Interfaces;

namespace AucaJobPortalDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly IProfileService _profileService;

    public ApplicationsController(
        IApplicationService applicationService,
        IProfileService profileService)
    {
        _applicationService = applicationService;
        _profileService = profileService;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(claim)) throw new Exception("User ID not found in token");
        return int.Parse(claim);
    }

    // ─── APPLY FOR A JOB ───
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Apply([FromBody] CreateApplicationDTO dto)
    {
        try
        {
            var userId = GetUserId();
            var application = await _applicationService.ApplyAsync(userId, dto);
            return Ok(new { success = true, message = "Application submitted successfully", data = application });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // ─── GET MY APPLICATIONS ───
    [HttpGet("my-applications")]
    [Authorize]
    public async Task<IActionResult> GetMyApplications()
    {
        try
        {
            var userId = GetUserId();
            var applications = await _applicationService.GetMyApplicationsAsync(userId);
            return Ok(new 
            { 
                success = true, 
                data = applications,
                count = applications.Count,
                message = applications.Count == 0 ? "You haven't applied to any jobs yet." : $"Found {applications.Count} application(s)."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // ─── GET APPLICATION DETAILS ───
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetApplicationById(int id)
    {
        try
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            return Ok(new { success = true, data = application });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    // ─── WITHDRAW APPLICATION ───
    [HttpPatch("{id}/withdraw")]
    [Authorize]
    public async Task<IActionResult> WithdrawApplication(int id)
    {
        try
        {
            var userId = GetUserId();
            await _applicationService.WithdrawApplicationAsync(id, userId);
            return Ok(new { success = true, message = "Application withdrawn successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // ─── GET APPLICATIONS FOR A JOB (Employer) ───
    [HttpGet("job/{jobId}")]
    [Authorize]
    public async Task<IActionResult> GetApplicationsForJob(int jobId)
    {
        try
        {
            var userId = GetUserId();
            var employerProfile = await _profileService.GetEmployerProfileAsync(userId);
            var applications = await _applicationService.GetApplicationsForJobAsync(jobId, employerProfile.Id);
            return Ok(new { success = true, data = applications, count = applications.Count });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // ─── UPDATE APPLICATION STATUS (Employer) ───
    [HttpPatch("{id}/status")]
    [Authorize]
    public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] UpdateApplicationStatusDTO dto)
    {
        try
        {
            var userId = GetUserId();
            var employerProfile = await _profileService.GetEmployerProfileAsync(userId);
            var application = await _applicationService.UpdateApplicationStatusAsync(id, employerProfile.Id, dto);
            return Ok(new 
            { 
                success = true, 
                message = $"Application status updated to {dto.Status}", 
                data = application 
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

     // ─── GET ALL APPLICATIONS FOR EMPLOYER ───
    [HttpGet("employer-applications")]
    [Authorize]
    public async Task<IActionResult> GetEmployerApplications()
    {
       try
           {
        var userId = GetUserId();
        var employerProfile = await _profileService.GetEmployerProfileAsync(userId);
        var applications = await _applicationService.GetEmployerApplicationsAsync(employerProfile.Id);
        return Ok(new 
        { 
            success = true, 
            data = applications,
            count = applications?.Count() ?? 0
        });
    }
    catch (Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}
}