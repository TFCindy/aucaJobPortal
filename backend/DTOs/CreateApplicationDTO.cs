using System.ComponentModel.DataAnnotations;

namespace AucaJobPortalDotNet.DTOs;

public class CreateApplicationDTO
{
    [Required]
    public int JobId { get; set; }
    public string? CoverLetter { get; set; }
    public string? ProfessionalExperience { get; set; }
    public string? Skills { get; set; }
    public int? CurrentYearOfStudy { get; set; }
    public bool UseExistingCv { get; set; } = true;  // Default: use existing
    public IFormFile? NewCvFile { get; set; } 
}