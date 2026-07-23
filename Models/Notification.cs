using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;
[Table("notifications")]
public class{
    [Column("id")]
    public int Id{get;private set;}
    [Column("user_id")]
    public int UserId{get; set;}
    [Column("title")]
    public string Title{get;set;}=string.Empty;
    [Column("message")]
    public string Message{get; set;}=string.Empty;
    [Column("type")]
    public string Type{get; set;}=string.Empty;
    [Column("related_application_id")]
    public int? RelatedApplicationId{get;set;}
    [Column("is_read")]
    public bool IsRead{get; set;}=false;
    [Column("created_At")]
    public DateTime CreatedAt{get;private set;}=DateTime.UtcNow;
    public virtual User User{get;set;}=null!;
    public virtual Application? Application{get;set;}
}