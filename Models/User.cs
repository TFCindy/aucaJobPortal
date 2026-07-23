using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Identity;

namespace AucaJobPortalDotNet.Models;
[Table("users")]
public class User
{
    [Column("id")]
    public int Id{get;private set;}
    [Column("email")]
    public string Email{get;set;}=string.Empty;
    [Column("password_hash")]
    public string PasswordHash{get;set;}=string.Empty;
    [Column("full_name")]
    public string FullName{get;set;}=string.Empty;
    [Column("phone")]
    public string? Phone{get;set;}
    [Column("is_active")]
    public bool IsActive{get;set;}=true;
    [Column("created_at")]
    public DateTime CreatedAt{get;private set;}=DateTime.UtcNow;
    [Column("updated_at")]
    public DateTime UpdatedAt{get;set;}=DateTime.UtcNow;//// Returns the current UTC time (no timezone offset)
    [Column("last_login_at")]
    public DateTime? LastLoginAt{get;set;}=DateTime.UtcNow;//datetime.now // Returns the current time on YOUR computer (in your timezone)
    [Column("password_reset_token")]
    public string? PasswordResetToken{get;set;}
    [Column("password_reset_expires")]
    public DateTime? PasswordResetExpires{set;get;}
    [Column("email_verified")]
    public bool EmailVerified{get;set;}=false;
    [Column("email_verification_token")]
    public string? EmailVerificationToken{get;set;}//can be null when the user email has already been verified or the token has expired
    [Column("email_verification_expires")]
    public DateTime? EmailVerifificationExpires{ get; set; } //can be null when No active token (email already verified)
    [Column("role_id")]
    public int RoleId{get;set;}
    public virtual Role Role{get;set;}=null!;
    public Student? Student{get;set;}

    public Employer? Employer{get;set;}

    public virtual ICollection<Notification> Notifications{get;set;}=[];
    public virtual ICollection<Message> SentMessages{get;set;}=[];
    public virtual ICollection<Message> ReceivedMessages{get;set;}=[];
    public virtual ICollection<PasswordResetLog> PasswordResetLogs{get;set;}=[];
    public virtual ICollection<EmailLog> EmailLogs{get;set;}=[];
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = [];
    public virtual ICollection<Application> ReviewedApplications{get;set;}=[];

}