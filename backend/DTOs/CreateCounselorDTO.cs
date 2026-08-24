using System.ComponentModel.DataAnnotations;

namespace AucaJobPortalDotNet.DTOs;

public class CreateCounselorDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Department { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Bio { get; set; }
    public string? Specialization { get; set; }
    public int? YearsOfExperience { get; set; }
    public string? OfficeLocation { get; set; }
    public string? PhoneExtension { get; set; }
}