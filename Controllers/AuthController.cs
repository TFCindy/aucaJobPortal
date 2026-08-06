using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AucaJobPortalDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(claim)) throw new Exception("User ID not found in token");
        return int.Parse(claim);
    }

    [HttpPost("register/student")]
    public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentDTO dto)
    {
        var result = await _authService.RegisterStudentAsync(dto);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPost("register/employer")]
    public async Task<IActionResult> RegisterEmployer([FromBody] RegisterEmployerDTO dto)
    {
        var result = await _authService.RegisterEmployerAsync(dto);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token)
    {
        var result = await _authService.VerifyEmailAsync(token);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
    {
        var response = await _authService.LoginAsync(dto);
        if (!response.Success)
        {
            return Unauthorized(response);
        }
        return Ok(response);
    }

     [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
    {
        var result = await _authService.ForgotPasswordAsync(dto);
        return Ok(result);
    }

    // ─── RESET PASSWORD ───
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
    {
        var result = await _authService.ResetPasswordAsync(dto);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    // ─── CHANGE PASSWORD (logged-in user) ───
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _authService.ChangePasswordAsync(userId, dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}