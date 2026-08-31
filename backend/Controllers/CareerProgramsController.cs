using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;
using AucaJobPortalDotNet.Models;

namespace AucaJobPortalDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CareerProgramsController : ControllerBase
{
    private readonly ICareerProgramService _programService;
    private readonly AucaDbContext _context;

    public CareerProgramsController(ICareerProgramService programService, AucaDbContext context)
    {
        _programService = programService;
        _context = context;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(claim)) throw new Exception("User ID not found in token");
        return int.Parse(claim);
    }

    // CREATE PROGRAM 
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProgram([FromForm] CreateCareerProgramDTO dto)
    {
        try
        {
            var userId = GetUserId();
            var program = await _programService.CreateProgramAsync(userId, dto);
            return Ok(new { success = true, message = "Program created successfully", data = program });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // GET: api/CareerPrograms/my-applications
[HttpGet("my-applications")]
[Authorize]
public async Task<IActionResult> GetMyProgramApplications()
{
    try
    {
        var userId = GetUserId();
        
        // Get the student from the UserId
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            return BadRequest(new { success = false, message = "Student profile not found" });
        }

        var applications = await _programService.GetMyProgramApplicationsAsync(student.Id);

        return Ok(new { success = true, data = applications, count = applications.Count });
    }
    catch (Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}

    // GET PROGRAM BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProgramById(int id)
    {
        try
        {
            var program = await _programService.GetProgramByIdAsync(id);
            return Ok(new { success = true, data = program });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    //  GET ALL PROGRAMS 
    [HttpGet]
    public async Task<IActionResult> GetAllPrograms([FromQuery] string? status = null, [FromQuery] string? type = null)
    {
        try
        {
            var programs = await _programService.GetAllProgramsAsync(status, type);
            return Ok(new { success = true, data = programs, count = programs.Count });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    //GET AVAILABLE PROGRAMS
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailablePrograms()
    {
        try
        {
            var programs = await _programService.GetAvailableProgramsAsync();
            return Ok(new { success = true, data = programs, count = programs.Count });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    //  GET MY PROGRAMS (Counselor)
    [HttpGet("my-programs")]
    [Authorize]
    public async Task<IActionResult> GetMyPrograms()
    {
        try
        {
            var userId = GetUserId();
            var programs = await _programService.GetProgramsByCounselorAsync(userId);
            return Ok(new { success = true, data = programs, count = programs.Count });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    //  UPDATE PROGRAM 
    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateProgram(int id, [FromForm] UpdateCareerProgramDTO dto)
    {
        try
        {
            var program = await _programService.UpdateProgramAsync(id, dto);
            return Ok(new { success = true, message = "Program updated successfully", data = program });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    //  PUBLISH PROGRAM
    [HttpPatch("{id}/publish")]
    [Authorize]
    public async Task<IActionResult> PublishProgram(int id)
    {
        try
        {
            await _programService.PublishProgramAsync(id);
            return Ok(new { success = true, message = "Program published successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // DELETE PROGRAM 
    [HttpPatch("{id}/delete")]
    [Authorize]
    public async Task<IActionResult> DeleteProgram(int id)
    {
        try
        {
            await _programService.DeleteProgramAsync(id);
            return Ok(new { success = true, message = "Program deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    //  APPLY TO PROGRAM (Student) 
    [HttpPost("{id}/apply")]
[Authorize]
public async Task<IActionResult> ApplyToProgram(int id, [FromBody] ProgramApplicationDTO dto)
{
    try
    {
        var userId = GetUserId();
        var student = await _context.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            return BadRequest(new { success = false, message = "Student profile not found" });
        }

        if (string.IsNullOrEmpty(dto.CurrentProgram))
        {
            dto.CurrentProgram = student.Program ?? "Unknown";
        }
        if (!dto.CurrentYearOfStudy.HasValue)
        {
            dto.CurrentYearOfStudy = student.YearOfStudy ?? 0;
        }

        var application = await _programService.ApplyToProgramAsync(student.Id, dto);

        return Ok(new { success = true, message = "Application submitted successfully", data = application });
    }
    catch (Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}

    //GET PROGRAM APPLICATIONS (Counselor) 
    [HttpGet("{id}/applications")]
    [Authorize]
    public async Task<IActionResult> GetProgramApplications(int id)
    {
        try
        {
            var applications = await _programService.GetProgramApplicationsAsync(id);
            return Ok(new { success = true, data = applications, count = applications.Count });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    //  UPDATE APPLICATION STATUS (Counselor)
    [HttpPatch("applications/{applicationId}/status")]
    [Authorize]
    public async Task<IActionResult> UpdateApplicationStatus(int applicationId, [FromBody] UpdateApplicationStatusDTO dto)
    {
        try
        {
            var application = await _programService.UpdateApplicationStatusAsync(applicationId, dto.Status);
            return Ok(new { success = true, message = $"Application status updated to {dto.Status}", data = application });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}