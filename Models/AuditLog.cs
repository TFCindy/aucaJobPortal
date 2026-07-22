using System;
using System.Collections.Generic;

namespace AucaJobPortalDotNet.Models;

public partial class AuditLog
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string EntityType { get; set; } = null!;

    public int? EntityId { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsSuccessful { get; set; }

    public string Severity { get; set; } = null!;

    public virtual User? User { get; set; }
}
