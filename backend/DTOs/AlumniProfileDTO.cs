namespace AucaJobPortalDotNet.DTOs;

public class AlumniProfileDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? ProfilePicture { get; set; }
    public string? Bio { get; set; }
    public string? Skills { get; set; }
    public string? CvFilePath { get; set; }
    public bool EmailNotificationsEnabled { get; set; }
    public string? Program { get; set; }
    public string? Faculty { get; set; }
    public string? Department { get; set; }
    public int GraduationYear { get; set; }
    public string DegreeClassification { get; set; } = string.Empty;
    public string? CurrentEmploymentStatus { get; set; }
    public string? CurrentEmployer { get; set; }
    public string? CurrentJobTitle { get; set; }
    public string? Industry { get; set; }
    public DateTime? EmploymentStartDate { get; set; }
    public bool TranscriptVerified { get; set; }
    public bool DiplomaVerified { get; set; }
    public string DocumentVerificationStatus { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? AlumniSince { get; set; }
}


public class UpdateAlumniProfileDTO
{
   
    public string? Phone { get; set; }
    public string? Bio { get; set; }
    public string? Skills { get; set; }
    public string? ProfilePicture { get; set; }
    public string? CvFilePath { get; set; }
    public bool? EmailNotificationsEnabled { get; set; }
     public string? Program { get; set; }
    public string? Faculty { get; set; }
    public string? Department { get; set; }
    public int? GraduationYear { get; set; }
    public string? DegreeClassification { get; set; }
    public string? CurrentEmploymentStatus { get; set; }
    public string? CurrentEmployer { get; set; }
    public string? CurrentJobTitle { get; set; }
    public string? Industry { get; set; }
    public DateTime? EmploymentStartDate { get; set; }
}