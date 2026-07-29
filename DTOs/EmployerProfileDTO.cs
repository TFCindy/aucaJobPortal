namespace AucaJobPortalDotNet.DTOs;

public class EmployerProfileDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? CompanyDescription { get; set; }
    public string? CompanyWebsite { get; set; }
    public string? Industry { get; set; }
    public string? CompanySize { get; set; }
    public string? Headquarters { get; set; }
    public string? CompanyLogo { get; set; }
    public bool Verified { get; set; }
    public bool EmailNotificationsEnabled { get; set; }
    public string? Phone { get; set; }
}

public class UpdateEmployerProfileDTO
{
    public string? CompanyName { get; set; }
    public string? CompanyDescription { get; set; }
    public string? CompanyWebsite { get; set; }
    public string? Industry { get; set; }
    public string? CompanySize { get; set; }
    public string? Location { get; set; }
    public string? Phone { get; set; }
}