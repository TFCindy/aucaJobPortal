using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;
[Table("job_types")]
public class JobType
{
    [Column("id")]
    public int Id {get;set;}
    [Column("name")]
    public string Name{get;set;}=string.Empty;
    [Column("description")]
    public string? Description{get;set;}
    [Column("created_at")]

    public DateTime CreatedAt{get;set;}
    [Column("group_name")]
    public string GroupName { get; set; } = "other";
    public virtual ICollection<JobJobType> JobJobTypes{get;set;}=[];
}