using System;
using System.Collections.Generic;

namespace AucaJobPortalDotNet.Models;

public partial class Application
{
    public int Id { get; private set; }

    public int JobId { get; set; }

    public int StudentId { get; set; }

    public string? CoverLetter { get; set; }

    public string Status { get; set; } = null!;

    public DateTime AppliedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? EmployerFeedback { get; set; }

    public DateTime? InterviewDate { get; set; }

    public int? ReviewedBy { get; set; }

    public string? CvFilePath { get; set; }

    public string? ProfessionalExperience { get; set; }

    public string? Skills { get; set; }

    public string? EducationLevel { get; set; }

    public string? EducationDescription { get; set; }

    public virtual Job Job { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual User? ReviewedByNavigation { get; set; }

    public virtual Student Student { get; set; } = null!;
}
