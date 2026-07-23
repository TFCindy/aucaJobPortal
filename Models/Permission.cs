using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;
[Table("permissions")]
public class Permission
{
    [Column("id")]
    public int Id{get;private set;}
    [Column("name")]
    public string Name{get;set;}=string.Empty;
    [Column("description")]
    public string? Description{get;set;}
    [Column("category")]
    public string? Category{get;set;}
   [Column("created_at")]
    public DateTime CreatedAt{get;private set;}
}