using System.ComponentModel.DataAnnotations;

namespace AucaJobPortalDotNet.DTOs;

public class UpdateApplicationStatusDTO
{
    [Required(AllowEmptyStrings = false)]
    public string Status { get; set; } = string.Empty;
    public string? EmployerFeedback { get; set; }
    public DateTime? InterviewDate { get; set; }
}