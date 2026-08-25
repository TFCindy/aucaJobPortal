namespace AucaJobPortalDotNet.DTOs;

public class UpdateCareerProgramDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public bool? IsExternal { get; set; }
    public string? ExternalOrganization { get; set; }
    public string? Type { get; set; }
    public string? Level { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? RegistrationDeadline { get; set; }
    public bool? IsOnline { get; set; }
    public string? Location { get; set; }
    public string? Venue { get; set; }
    public string? MeetingLink { get; set; }
    public int? MaxParticipants { get; set; }
    public bool? IsFree { get; set; }
    public decimal? Cost { get; set; }
    public string? Status { get; set; }
    public bool? IsFeatured { get; set; }
    public string? ImageUrl { get; set; }
}