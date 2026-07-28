namespace AucaJobPortalDotNet.DTOs;

public class StudentProfileDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string Program { get; set; } = string.Empty;
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
    public string? ProfilePicture { get; set; }
    public string? CvFilePath { get; set; }
    public bool EmailNotificationsEnabled { get; set; }
}