namespace AucaJobPortalDotNet.Models;
public class Student
{
    public int Id{get;private set;}
    public int UserId{get;set;}
    public int StudentId{get;set;}
    public string Program{get;set;}=string.Empty;
    public int YearOfStudy{get;set;}
    public int GraduationYear{get;set;}
    public string? CvFilePath{get;set;}
    public string? Skills{get;set;}
    public string? ProfilePicture{get;set;}
    public string? Bio{get;set;}
    public DateTime CreatedAt{get;private set;}=DateTime.UtcNow;
    public string? Faculty{get;set;}
    public string? Department{get;set;}
    public bool EmailNotificationsEnabled{get;set;}=true;
    public virtual User User{get;set;}=null!;
}