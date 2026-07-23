using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;

[Table("role")]
public class Role
{
    [Column("id")]
    public int Id{get;private set;}
    [Column("name")]
    public string Name{get;set;}=string.Empty;
    [Column("description")]
    public string Description{get;set;}=string.Empty;
    [Column("is_system_role")]
    public bool IsSystemRole{get;set;}=false;//when u create a new role,it is user created not system/built_in
    [Column("created_at")]
    public DateTime CreatedAt{get;private set;}=DateTime.UtcNow;
}