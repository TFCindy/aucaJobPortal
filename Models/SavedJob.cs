using System;
using System.Collections.Generic;

namespace AucaJobPortalDotNet.Models;

public partial class SavedJob
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int JobId { get; set; }

    public DateTime? SavedAt { get; set; }

    public virtual Job Job { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
