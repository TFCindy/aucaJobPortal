namespace AucaJobPortalDotNet.Models;
public class Job
{
    public int Id{get;private set;}
    public int EmployerId{get;set;}
    public string Title{get;set;}=string.Empty;
    public string Description{get;set;}=string.Empty;
    public string? Requirements{get;set;}
    public string? Responsibilities{get;set;}
    public string? Location{get;set;}
    public string? JobType{get;set;}
    public string? SalaryRange{get;set;}
    public string? ExperienceLevel{get;set;}
    public DateTime? DeadlineDate{get;set;}
    public string Status{get;set;}="";
    public DateTime PostedAt{get;private set;}=DateTime.UtcNow;
    public DateTime UpdatedAt{get;set;}=DateTime.UtcNow;
    public int? ApprovedBy{get;set;}//null bcoz it  might have not yet been reviewed
    public DateTime? ApprovedAt{get;set;}
    public int ViewsCount{get;set;}
    public DateTime? ExpiresAt{get;set;}
    public int Openings{get;set;}=1;
    public virtual Employer Employer{get;set;}=null!;
    public virtual User? ApprovedByNavigation{get;set;}

}