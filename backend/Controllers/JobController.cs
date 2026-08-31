using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;
using AucaJobPortalDotNet.Services.Interfaces;

namespace AucaJobPortalDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;
    private readonly IProfileService _profileService;
    private readonly IPermissionService _permissionService;

    public JobsController(IJobService jobService, IProfileService profileService, IPermissionService permissionService)
    {
        _jobService = jobService;
        _profileService = profileService;
        _permissionService = permissionService;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(claim)) throw new Exception("User ID not found in token");
        return int.Parse(claim);
    }

    private int GetRoleId()
    {
        var claim = User.FindFirst("roleId")?.Value;
        if (string.IsNullOrEmpty(claim)) throw new Exception("Role ID not found in token");
        return int.Parse(claim);
    }

   // ─── PUBLIC: Get all active jobs (no filters) ───
    [HttpGet("public")]
    public async Task<IActionResult> GetPublicJobs()
    {
        try
        {
            var jobs = await _jobService.GetActiveJobsAsync();
            return Ok(new
            {
                success = true,
                data = jobs,
                count = jobs.Count,
                message = jobs.Count == 0 
                    ? "No active jobs available." 
                    : $"Found {jobs.Count} job(s). Login to access advanced search filters."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // ─── PROTECTED: Search jobs with filters ───
    [HttpGet("search")]
[Authorize]
public async Task<IActionResult> SearchJobs(
    [FromQuery] List<int>? jobTypeIds,  // ← Now supports multiple!
    [FromQuery] string? location)
{
    var jobs = await _jobService.SearchJobsAsync(jobTypeIds, location);
    return Ok(new { success = true, data = jobs, count = jobs.Count });
}

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobById(int id)
    {
        try
            {
        var job = await _jobService.GetJobByIdAsync(id);
        return Ok(new { success = true, data = job });
    }
        catch (Exception ex)
    {
        return NotFound(new { success = false, message = ex.Message });
    }
    }

    // ─── GET MY JOBS (Employer only) ───
    [HttpGet("my-jobs")]
    [Authorize]
    public async Task<IActionResult> GetMyJobs()
    {
        try
        {
            var jobs = await _jobService.GetJobsByEmployerUserIdAsync(GetUserId());
            return Ok(new
            {
                success = true,
                message = jobs.Count == 0 ? "You haven't posted any jobs yet." : $"Found {jobs.Count} job(s).",
                data = jobs,
                count = jobs.Count
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // GET PENDING JOBS (Admin only)
    [HttpGet("pending-approval")]
    [Authorize]
    public async Task<IActionResult> GetPendingJobs()
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "approve_jobs"))
            {
                return Forbid();
            }

            var jobs = await _jobService.GetAllJobsAsync();
            var pending = jobs.Where(j => j.Status == "pending").ToList();

            return Ok(new
            {
                success = true,
                message = pending.Count == 0 ? "No jobs are currently pending approval." : $"{pending.Count} job(s) pending approval.",
                data = pending,
                count = pending.Count
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // ─── CREATE JOB (Employer only) ───
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobDTO dto)
    {
        try
        {
            // ─── INPUT VALIDATION ───
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return BadRequest(new { success = false, message = "Job title is required." });
            }

            if (string.IsNullOrWhiteSpace(dto.Description))
            {
                return BadRequest(new { success = false, message = "Job description is required." });
            }

            if (dto.JobTypeIds == null || !dto.JobTypeIds.Any())
            {
                return BadRequest(new { success = false, message = "At least one job type is required." });
            }

            if (dto.Openings < 1 || dto.Openings > 100)
            {
                return BadRequest(new { success = false, message = "Openings must be between 1 and 100." });
            }

            if (dto.DeadlineDate.HasValue && dto.DeadlineDate.Value.Date <= DateTime.UtcNow.Date)
            {
                return BadRequest(new { success = false, message = "Deadline must be a future date." });
            }

            // ─── BUSINESS LOGIC ───
            var employerProfile = await _profileService.GetEmployerProfileAsync(GetUserId());
            var job = await _jobService.CreateJobAsync(dto, employerProfile.Id);
            
            return Ok(new { success = true, message = "Job submitted for approval", data = job });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // ─── UPDATE JOB (Employer only) ───
    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateJob(int id, [FromBody] UpdateJobDTO dto)
    {
        try
        {
            var job = await _jobService.UpdateJobAsync(id, dto);
            return Ok(new { success = true, message = "Job updated successfully", data = job });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    // ─── WITHDRAW JOB (Employer only) ───
    [HttpPatch("{id}/withdraw")]
    [Authorize]
    public async Task<IActionResult> WithdrawJob(int id)
    {
        try
        {
            await _jobService.WithdrawJobAsync(id);
            return Ok(new { success = true, message = "Job withdrawn successfully" });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    // ─── APPROVE JOB (Admin only) ───
    [HttpPatch("{id}/approve")]
    [Authorize]
    public async Task<IActionResult> ApproveJob(int id)
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "approve_jobs"))
            {
                return Forbid();
            }

            var job = await _jobService.ApproveJobAsync(id, GetUserId());
            return Ok(new { success = true, message = "Job approved successfully", data = job });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    // ─── REJECT JOB (Admin only) ───
    [HttpPatch("{id}/reject")]
    [Authorize]
    public async Task<IActionResult> RejectJob(int id)
    {
        try
        {
            if (!await _permissionService.HasPermissionAsync(GetRoleId(), "approve_jobs"))
            {
                return Forbid();
            }

            var job = await _jobService.RejectJobAsync(id, GetUserId());
            return Ok(new { success = true, message = "Job rejected", data = job });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }
    
    // ─── SAVE JOB ───
[HttpPost("{id}/save")]
[Authorize]
public async Task<IActionResult> SaveJob(int id)
{
    try
    {
        var userId = GetUserId();
        await _jobService.SaveJobAsync(id, userId);
        return Ok(new { success = true, message = "Job saved successfully" });
    }
    catch (Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}

// ─── UNSAVE JOB ───
[HttpDelete("{id}/save")]
[Authorize]
public async Task<IActionResult> UnsaveJob(int id)
{
    try
    {
        var userId = GetUserId();
        await _jobService.UnsaveJobAsync(id, userId);
        return Ok(new { success = true, message = "Job unsaved successfully" });
    }
    catch (Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}

// ─── GET SAVED JOBS ───
[HttpGet("saved")]
[Authorize]
public async Task<IActionResult> GetSavedJobs()
{
    try
    {
        var userId = GetUserId();
        var jobs = await _jobService.GetSavedJobsAsync(userId);
        return Ok(new { success = true, data = jobs });
    }
    catch (Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}
}