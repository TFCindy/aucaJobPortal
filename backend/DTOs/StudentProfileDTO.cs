namespace AucaJobPortalDotNet.DTOs;

public class StudentProfileDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string? Program { get; set; } 
    public string? Faculty { get; set; }
    public string? Department { get; set; }
    public int? YearOfStudy { get; set; }
    public int? GraduationYear { get; set; }
    public string? Phone { get; set; }
    public string? Skills { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePicture { get; set; }
    public string? CvFilePath { get; set; }
    public bool EmailNotificationsEnabled { get; set; }
     public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
     public string? DegreeClassification { get; set; }
    public string? CurrentEmploymentStatus { get; set; }
    public string? CurrentEmployer { get; set; }
    public string? CurrentJobTitle { get; set; }
    public string? Industry { get; set; }
    public DateTime? EmploymentStartDate { get; set; }
    public string? TranscriptFilePath { get; set; }
    public string? DiplomaFilePath { get; set; }
    public bool IsAlumni { get; set; }
}

public class UpdateStudentProfileDTO
{
    public string? Program { get; set; }
    public string? Faculty { get; set; }
    public string? Department { get; set; }
    public int? YearOfStudy { get; set; }
    public int? GraduationYear { get; set; }
    public string? Phone { get; set; }
    public string? Skills { get; set; }
    public string? Bio { get; set; }
    public IFormFile? ProfilePicture { get; set; }
    public IFormFile? CvFile { get; set; }
    public bool EmailNotificationsEnabled { get; set; }
     public string? DegreeClassification { get; set; }
    public string? CurrentEmploymentStatus { get; set; }
    public string? CurrentEmployer { get; set; }
    public string? CurrentJobTitle { get; set; }
    public string? Industry { get; set; }
    public DateTime? EmploymentStartDate { get; set; }
    public IFormFile? TranscriptFile { get; set; }
    public IFormFile? DiplomaFile { get; set; }

   public bool IsAlumni { get; set; }
}