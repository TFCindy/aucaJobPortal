using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;
[Table("jobs")]
public class Job
{
    [Column("id")]
    public int Id{get;private set;}
    [Column("employer_id")]
    public int EmployerId{get;set;}
    [Column("title")]
    public string Title{get;set;}=string.Empty;
    [Column("description")]
    public string Description{get;set;}=string.Empty;
    [Column("requirements")]
    public string? Requirements{get;set;}
    [Column("responsibilities")]
    public string? Responsibilities{get;set;}
    [Column("location")]
    public string? Location{get;set;}
    [Column("job_type")]
    public string? JobType{get;set;}
    [Column("salary_range")]
    public string? SalaryRange{get;set;}
    [Column("experience_level")]
    public string? ExperienceLevel{get;set;}
    [Column("deadline")]
    public DateTime? DeadlineDate{get;set;}
    [Column("status")]
    public string Status{get;set;}="";
    [Column("posted_at")]
    public DateTime PostedAt{get;private set;}=DateTime.UtcNow;
    [Column("updated_at")]
    public DateTime UpdatedAt{get;set;}=DateTime.UtcNow;
    [Column("approved_by")]
    public int? ApprovedBy{get;set;}//null bcoz it  might have not yet been reviewed
    [Column("approved_at")]
    public DateTime? ApprovedAt{get;set;}
    [Column("views_count")]
    public int ViewsCount{get;set;}
    [Column("expires_at")]
    public DateTime? ExpiresAt{get;set;}
    [Column("openings")]
    public int Openings{get;set;}=1;
    public virtual Employer Employer{get;set;}=null!;
    public virtual User? ApprovedByNavigation{get;set;}

}