namespace AucaJobPortalDotNet.DTOs;

public class CareerCounselorProfileDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? Specialization { get; set; }
    public int? YearsOfExperience { get; set; }
    public string? OfficeLocation { get; set; }
    public bool IsActive { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
}

public class UpdateCareerCounselorProfileDTO
{
    public string? Department { get; set; }
    public string? Title { get; set; }
    public string? Bio { get; set; }
    public string? Specialization { get; set; }
    public int? YearsOfExperience { get; set; }
    public string? OfficeLocation { get; set; }
    public string? Phone { get; set; }
}
