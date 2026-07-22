using System;
using System.Collections.Generic;

namespace AucaJobPortalDotNet.Models;

public partial class Job
{
    public int Id { get; private set; }

    public int EmployerId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Requirements { get; set; }

    public string? Responsibilities { get; set; }

    public string? Location { get; set; }

    public string? JobType { get; set; }

    public string? SalaryRange { get; set; }

    public string? ExperienceLevel { get; set; }

    public DateOnly? Deadline { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? PostedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public int ViewsCount { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public int Openings { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual Employer Employer { get; set; } = null!;

    public virtual ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
}
