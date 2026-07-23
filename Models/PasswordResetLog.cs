using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;
[Table("password_reset_logs")]
public class PasswordResetLog
{
    [Column("id")]
    public int Id{get;private set;}
    [Column("user_id")]
    public int? UserId{get;set;}
    [Column("email")]
    public string Email{get;private set;}=string.Empty;
    [Column("reset_requested_at")]
    public DateTime ResetRequestedAt{get;private set;}=DateTime.UtcNow;
    
    [Column("reset_completed_at")]
    public DateTime? ResetCompletedAt{get;set;}
    
    [Column("ip_address")]
    public string? IpAddress{get;set;}
    [Column("user_agent")]
    public string? UserAgent{get;set;}
    public User? User{get;set;}
    

}