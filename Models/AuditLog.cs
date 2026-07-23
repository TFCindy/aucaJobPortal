using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;
[Table("audit_logs")]
public class AuditLog
{
    [Column("id")]
    public int Id{get;private set;}
    [Column("user_id")]
    public int? UserId{get; set;}
    [Column("action")]
    public string Action{get;set;}=string.Empty;
    [Column("entity_type")]
    public string EntityType{get;set;}=string.Empty;
    [Column("entity_id")]
    public int? EntityId{get;set;}
    [Column("old_value")]
    public string? OldValue{get;set;}
    [Column("new_value")]
    public string? NewValue{get;set;}
    [Column("ip_address")]
    public string? IpAddress{get;set;}
    [Column("user_agent")]
    public string? UserAgent{get;set;}
    [Column("created_at")]
    public DateTime CreatedAt{get;private set;}=DateTime.UtcNow;
    [Column("is_successful")]
    public bool IsSuccessful{get;set;}=true;
    [Column("severity")]
    public string Severity{get;set;}=string.Empty;
    public virtual User? User{get;set;}

}