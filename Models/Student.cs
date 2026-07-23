using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;
[Table("students")]
public class Student
{
    [Column("id")]
    public int Id{get;private set;}
    [Column("user_id")]
    public int UserId{get;set;}
    [Column("student_id")]
    public string StudentId{get;set;}=string.Empty;
    [Column("program")]
    public string Program{get;set;}=string.Empty;
    [Column("year_of_study")]
    public int? YearOfStudy{get;set;}
    [Column("graduation_year")]
    public int? GraduationYear{get;set;}
    [Column("cv_file_path")]
    public string? CvFilePath{get;set;}
    [Column("skills")]
    public string? Skills{get;set;}
    [Column("profile_picture")]
    public string? ProfilePicture{get;set;}
    [Column("bio")]
    public string? Bio{get;set;}
    [Column("created_at")]
    public DateTime CreatedAt{get;private set;}=DateTime.UtcNow;
    [Column("faculty")]
    public string? Faculty{get;set;}
    [Column("department")]
    public string? Department{get;set;}
    [Column("email_notifications_enabled")]
    public bool EmailNotificationsEnabled{get;set;}=true;
    public virtual User User{get;set;}=null!;
}