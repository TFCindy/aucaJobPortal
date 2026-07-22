using System;
using System.Collections.Generic;

namespace AucaJobPortalDotNet.Models;

public partial class PasswordResetLog
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Email { get; set; } = null!;

    public DateTime? ResetRequestedAt { get; set; }

    public DateTime? ResetCompletedAt { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public virtual User? User { get; set; }
}
