using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Services.Interfaces;

namespace AucaJobPortalDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // All endpoints require authentication
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new Exception("User ID not found in token");
        }
        return int.Parse(userIdClaim);
    }

    // ─── GET STUDENT PROFILE ───
    [HttpGet("student")]
    public async Task<IActionResult> GetStudentProfile()
    {
        try
        {
            var userId = GetUserId();
            var profile = await _profileService.GetStudentProfileAsync(userId);
            return Ok(new { success = true, data = profile });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    //UPDATE STUDENT PROFILE
    [HttpPatch("student")]
[Authorize]
public async Task<IActionResult> UpdateStudentProfile([FromForm] UpdateStudentProfileDTO dto)
{
    try
    {
        var userId = GetUserId();
        var profile = await _profileService.UpdateStudentProfileAsync(userId, dto);
        return Ok(new { success = true, message = "Profile updated successfully", data = profile });
    }
    catch (Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}

    // GET EMPLOYER PROFILE
    [HttpGet("employer")]
    public async Task<IActionResult> GetEmployerProfile()
    {
        try
        {
            var userId = GetUserId();
            var profile = await _profileService.GetEmployerProfileAsync(userId);
            return Ok(new { success = true, data = profile });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    //  UPDATE EMPLOYER PROFILE
    [HttpPatch("employer")]
    public async Task<IActionResult> UpdateEmployerProfile([FromForm] UpdateEmployerProfileDTO dto)
    {
        try
        {
            var userId = GetUserId();
            var profile = await _profileService.UpdateEmployerProfileAsync(userId, dto);
            return Ok(new { success = true, message = "Profile updated successfully", data = profile });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    // ─── GET ALUMNI PROFILE ───
[HttpGet("alumni")]
public async Task<IActionResult> GetAlumniProfile()
{
    try
    {
        var userId = GetUserId();
        var profile = await _profileService.GetAlumniProfileAsync(userId);
        return Ok(new { success = true, data = profile });
    }
    catch (Exception ex)
    {
        return NotFound(new { success = false, message = ex.Message });
    }
}

// ─── UPDATE ALUMNI PROFILE ───
[HttpPatch("alumni")]
public async Task<IActionResult> UpdateAlumniProfile([FromBody] UpdateAlumniProfileDTO dto)
{
    try
    {
        var userId = GetUserId();
        var profile = await _profileService.UpdateAlumniProfileAsync(userId, dto);
        return Ok(new { success = true, message = "Alumni profile updated successfully", data = profile });
    }
    catch (Exception ex)
    {
        return NotFound(new { success = false, message = ex.Message });
    }
}

// ─── GET CAREER COUNSELOR PROFILE ───
[HttpGet("counselor")]
public async Task<IActionResult> GetCareerCounselorProfile()
{
    try
    {
        var userId = GetUserId();
        var profile = await _profileService.GetCareerCounselorProfileAsync(userId);
        return Ok(new { success = true, data = profile });
    }
    catch (Exception ex)
    {
        return NotFound(new { success = false, message = ex.Message });
    }
}

// ─── UPDATE CAREER COUNSELOR PROFILE ───
[HttpPatch("counselor")]
public async Task<IActionResult> UpdateCareerCounselorProfile([FromBody] UpdateCareerCounselorProfileDTO dto)
{
    try
    {
        var userId = GetUserId();
        var profile = await _profileService.UpdateCareerCounselorProfileAsync(userId, dto);
        return Ok(new { success = true, message = "Profile updated successfully", data = profile });
    }
    catch (Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}
}