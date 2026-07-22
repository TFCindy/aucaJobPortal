using System;
using System.Collections.Generic;

namespace AucaJobPortalDotNet.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public string? PasswordResetToken { get; set; }

    public DateTime? PasswordResetExpires { get; set; }

    public bool EmailVerified { get; set; }

    public string? EmailVerificationToken { get; set; }

    public DateTime? EmailVerificationExpires { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<EmailLog> EmailLogs { get; set; } = new List<EmailLog>();

    public virtual ICollection<Employer> Employers { get; set; } = new List<Employer>();

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PasswordResetLog> PasswordResetLogs { get; set; } = new List<PasswordResetLog>();

    public virtual Role Role { get; set; } = null!;//= null! = "starts empty, and I'm telling the compiler to stop complaining about that"

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();//virtual = "this is allowed to auto-fill itself later" (safe to ignore for now)

}
