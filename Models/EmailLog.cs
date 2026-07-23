using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;
[Table("email_logs")]
public class EmailLogs
{
    [Column("id")]
    public int Id{get;private set;}
    [Column("recipient_email")]
    public string RecipientEmail{get; set;}=string.Empty;
    [Column("email_type")]
    public string EmailType{get;set;}=string.Empty;
    [Column("subject")]
    public string? Subject{get; set;}
    [Column("status")]
    public string Status{get;set;}=string.Empty;
    [Column("error_message")]
    public string? ErrorMessage{get;set;}
    [Column("sent_at")]
    public DateTime SentAt{get; set;}=DateTime.UtcNow;
    [Column("user_id")]
    public int? UserId{get;set;}
    public User? User{get;set;}
}