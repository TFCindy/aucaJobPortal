namespace AucaJobPortalDotNet.DTOs;

public class ProgramApplicationResponseDTO
{
    public int Id { get; set; }
    public int ProgramId { get; set; }
    public string ProgramTitle { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? MotivationLetter { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public string? StudentProgram { get; set; }
    public int? StudentYearOfStudy { get; set; }
    public string StudentProgramAtApplication { get; set; } = string.Empty; 
    public int StudentYearAtApplication { get; set; } 
}