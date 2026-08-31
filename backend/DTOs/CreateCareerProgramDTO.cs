using System.ComponentModel.DataAnnotations;

namespace AucaJobPortalDotNet.DTOs;

public class CreateCareerProgramDTO
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public string? ShortDescription { get; set; }

    public bool IsExternal { get; set; } = false;
    public string? ExternalOrganization { get; set; }

    [Required]
    public string Type { get; set; } = string.Empty;
    // "training", "workshop", "seminar", "webinar", "bootcamp"

    public string? Level { get; set; }
    // "beginner", "intermediate", "advanced", "all_levels"

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public DateTime RegistrationDeadline { get; set; }

    public bool IsOnline { get; set; } = false;
    public string? Location { get; set; }
    public string? Venue { get; set; }
    public string? MeetingLink { get; set; }

    [Required]
    [Range(1, 1000)]
    public int MaxParticipants { get; set; }

    public bool IsFree { get; set; } = true;
    public decimal? Cost { get; set; }
    public IFormFile? Image { get; set; }
    public DateTime PublishedAt{get;set;}

    public string? Status { get; set; }
}

