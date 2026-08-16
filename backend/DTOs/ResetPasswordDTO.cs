using System.ComponentModel.DataAnnotations;

namespace AucaJobPortalDotNet.DTOs;

public class ResetPasswordDTO
{
    [Required]
    public string Token { get; set; } = string.Empty; 

    [Required, MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [Compare("NewPassword")]
    public string ConfirmPassword { get; set; } = string.Empty;
}