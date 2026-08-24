namespace AucaJobPortalDotNet.DTOs;

public class CounselorResponseDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? Specialization { get; set; }
    public int? YearsOfExperience { get; set; }
    public string? OfficeLocation { get; set; }
    public string? PhoneExtension { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
}