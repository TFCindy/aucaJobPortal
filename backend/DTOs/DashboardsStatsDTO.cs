namespace AucaJobPortalDotNet.DTOs;

public class DashboardStatsDTO
{
    public int TotalUsers { get; set; }
    public int TotalJobs { get; set; }
    public int PendingJobs { get; set; }
    public int TotalCounselors { get; set; }
    public List<RecentUserDTO> RecentUsers { get; set; } = new();
    public List<RecentJobDTO> RecentJobs { get; set; } = new();
}

public class RecentUserDTO
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string Role { get; set; } = string.Empty;
}

public class RecentJobDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
}