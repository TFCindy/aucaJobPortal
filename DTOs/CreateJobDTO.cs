using System.ComponentModel.DataAnnotations;

namespace AucaJobPortalDotNet.DTOs;

public class CreateJobDTO
{
    [Required(AllowEmptyStrings = false), MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; } = string.Empty;

    public string? Requirements { get; set; }
    public string? Responsibilities { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Location { get; set; } = string.Empty;

    [Required, MinLength(1, ErrorMessage = "At least one job type must be selected")]
    public List<int> JobTypeIds { get; set; } = [];

    public DateTime? DeadlineDate { get; set; }

    [Required, Range(1, 100, ErrorMessage = "Openings must be between 1 and 100")]
    public int Openings { get; set; }
}