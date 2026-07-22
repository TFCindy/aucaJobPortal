using System;
using System.Collections.Generic;

namespace AucaJobPortalDotNet.Models;

public partial class Employer
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? CompanyDescription { get; set; }

    public string? CompanyWebsite { get; set; }

    public string Industry { get; set; } = null!;

    public string? CompanySize { get; set; }

    public string? Headquarters { get; set; }

    public string? CompanyLogo { get; set; }

    public bool Verified { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? VerificationDocumentPath { get; set; }

    public bool EmailNotificationsEnabled { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual User User { get; set; } = null!;
}
