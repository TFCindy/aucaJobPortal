namespace AucaJobPortalDotNet.DTOs;

public class ProgramApplicationResponseDTO
{
    public int Id { get; set; }
    public int ProgramId { get; set; }
    public string ProgramTitle { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public string? StudentProgram { get; set; }
    public int? StudentYearOfStudy { get; set; }
    public string? MotivationLetter { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool Attended { get; set; }
    public bool Completed { get; set; }
    public string? CompletionCertificate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}