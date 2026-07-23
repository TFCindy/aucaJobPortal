namespace AucaJobPortalDotNet.Models;
public class Employer
{
    public int Id{get;private set;}
    public int UserId{get;set;}
    public string CompanyName{get;set;}=string.Empty;
    public string? CompanyDescription{get;set;}
    public string? CompanyWebsite{get;set;}
    public string Industry{get;set;}=string.Empty;
    public string? CompanySize{get;set;}
    public string? Headquarters{get;set;}
    public string? CompanyLogo{get;set;}
    public bool Verified{get;set;}=false;
    public DateTime CreatedAt{get;private set;}=DateTime.UtcNow;
    public string? VerificationDocumentPath{get;set;}
    public bool EmailNotificationsEnabled{get;set;}=true;

    public User User{get;set;}=null!;
}