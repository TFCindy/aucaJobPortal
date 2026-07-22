namespace AucaJobPortalDotNet.Models;
public class Application
{
    public int Id{get;private set;}
    public int JobId{get;set;}
    public int StudentId{get;set;}
    public string? CoverLetter{get;set;}
    public string Status{get;set;}="pending";
    public DateTime AppliedAt{get;private set;}=DateTime.UtcNow;
    public DateTime UpdatedAt{get; set;}=DateTime.UtcNow;
    public string? EmployerFeedback{get;set;}
    public DateTime? InterviewDate{get;set;}
    public string? ReviewedBy{get;set;}
    public string? CvFilePath{get;set;}
    public string? ProfessionalExperience{get;set;}
    public string? Skills{get;set;}
    public string? EducationLevel{get;set;}
    public string? EducationDescription{get;set;}
    public virtual Job Job{get;set;}=null!;//job is not null,meaning each application shud belong to a job,so we make it not null and assure the compiler that it wiill never be null
    public virtual Student Student{get;set;}=null;//Student shud never be null coz each application beloongs to a student
    public virtual User? ReviewedByNavigation{get;set;}//may be null

}