using System;
using System.Collections.Generic;

namespace AucaJobPortalDotNet.Models;

public partial class EmailLog
{
    public int Id { get; set; }

    public string RecipientEmail { get; set; } = null!;

    public string EmailType { get; set; } = null!;

    public string? Subject { get; set; }

    public string Status { get; set; } = null!;

    public string? ErrorMessage { get; set; }

    public DateTime? SentAt { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
