namespace AucaJobPortalDotNet.DTOs;

public class JobResponseDTO
{
    public int Id { get; set; }
    public int EmployerId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Requirements { get; set; }
    public string? Responsibilities { get; set; }
    public string? Location { get; set; }
    public DateTime? Deadline { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime PostedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int ViewsCount { get; set; }
    public int Openings { get; set; }
    public int? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public bool IsActive => Status == "active";
    public List<string> JobTypes { get; set; } = new();
}