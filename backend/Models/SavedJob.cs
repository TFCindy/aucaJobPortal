using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;
[Table("saved_jobs")]
public class SavedJob
{
    [Column("id")]
    public int Id{get;private set;}
    [Column("student_id")]
    public int StudentId{get;set;}
    [Column("job_id")]
    public int JobId{get; set;}
    [Column("saved_at")]
    public DateTime SavedAt{get;private set;}=DateTime.UtcNow;
    public virtual Student Student{get;set;}=null!;

    public virtual Job Job{get;set;}=null!;
}