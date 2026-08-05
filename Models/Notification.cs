using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;

[Table("notifications")]
public class Notification
{
    [Column("id")]
    public int Id { get; private set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("message")]
    public string Message { get; set; } = string.Empty;

    [Column("type")]
    public string Type { get; set; } = string.Empty;

    [Column("is_read")]
    public bool IsRead { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    [Column("entity_type")]
    public string? EntityType { get; set; }

    [Column("entity_id")]
    public int? EntityId { get; set; }

    public virtual User User { get; set; } = null!;
}