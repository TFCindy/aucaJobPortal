using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;
[Table("appliactions")]
public class Application
{
    [Column("id")]
    public int Id{get;private set;}
    [Column("job_id")]
    public int JobId{get;set;}
    [Column("student_id")]
    public int StudentId{get;set;}
    [Column("cover_letter")]
    public string? CoverLetter{get;set;}
    [Column("status")]
    public string Status{get;set;}="pending";
    [Column("applied_at")]
    public DateTime AppliedAt{get;private set;}=DateTime.UtcNow;
    [Column("updated_at")]
    public DateTime UpdatedAt{get; set;}=DateTime.UtcNow;
    [Column("employer_feedback")]
    public string? EmployerFeedback{get;set;}
    [Column("interview_date")]
    public DateTime? InterviewDate{get;set;}
    [Column("reviewed_by")]
    public string? ReviewedBy{get;set;}
    [Column("cv_file_path")]
    public string? CvFilePath{get;set;}
    [Column("professional_experience")]
    public string? ProfessionalExperience{get;set;}
    [Column("skills")]
    public string? Skills{get;set;}
    [Column("education_level")]
    public string? EducationLevel{get;set;}
    [Column("education_description")]
    public string? EducationDescription{get;set;}
    public virtual Job Job{get;set;}=null!;//job is not null,meaning each application shud belong to a job,so we make it not null and assure the compiler that it wiill never be null
    public virtual Student Student{get;set;}=null!;//Student shud never be null coz each application beloongs to a student
    public virtual User? ReviewedByNavigation{get;set;}//may be null

}