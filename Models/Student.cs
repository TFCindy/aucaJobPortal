using System;
using System.Collections.Generic;

namespace AucaJobPortalDotNet.Models;

public partial class Student
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string StudentId { get; set; } = null!;

    public string Program { get; set; } = null!;

    public int? YearOfStudy { get; set; }

    public int? GraduationYear { get; set; }

    public string? CvFilePath { get; set; }

    public string? Skills { get; set; }

    public string? ProfilePicture { get; set; }

    public string? Bio { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Faculty { get; set; }

    public string? Department { get; set; }

    public bool EmailNotificationsEnabled { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();

    public virtual User User { get; set; } = null!;
}
