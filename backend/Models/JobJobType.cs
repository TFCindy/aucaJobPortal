using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;
[Table("job_job_types")]
public class JobJobType
{
    [Column("job_id")]
    public int JobId{get;set;}
    [Column("job_type_id")]
    public int JobTypeId{get;set;}
    public virtual Job Job { get; set; } = null!;        
    public virtual JobType JobType { get; set; } = null!;
}