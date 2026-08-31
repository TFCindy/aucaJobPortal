namespace AucaJobPortalDotNet.DTOs;

public class ApplicationResponseDTO
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string StudentId{get;set;}=string.Empty;
    public string CompanyName { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;
    public int StudentIdNumber { get; set; }  
    public string StudentEmail { get; set; } = string.Empty;
    public string? StudentPhone { get; set; }
    public string? Program { get; set; }
    public int? YearOfStudy { get; set; }

    public string? CoverLetter { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime AppliedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? EmployerFeedback { get; set; }
    public DateTime? InterviewDate { get; set; }
    public string? CvFilePath { get; set; }
    public string? ProfessionalExperience { get; set; }
    public string? Skills { get; set; }
    public int? StudentYearAtApplication { get; set; }

}