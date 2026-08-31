namespace AucaJobPortalDotNet.DTOs;

public class CareerProgramResponseDTO
{
    public int Id { get; set; }
    public int CareerCounselorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public bool IsExternal { get; set; }
    public string? ExternalOrganization { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? Level { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime RegistrationDeadline { get; set; }
    public bool IsOnline { get; set; }
    public string? Location { get; set; }
    public string? Venue { get; set; }
    public string? MeetingLink { get; set; }
    public int MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
    public bool IsFree { get; set; }
    public decimal? Cost { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public string? ImageUrl { get; set; }
    public int ApplicationCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Computed properties
    public bool IsFull { get; set; }
    public bool IsRegistrationOpen { get; set; }
    public int AvailableSpots { get; set; }

    // Counselor info
    public string CounselorName { get; set; } = string.Empty;
    public string CounselorDepartment { get; set; } = string.Empty;
    
}