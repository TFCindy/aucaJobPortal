using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;

[Table("employers")]
public class Employer
{
    [Column("id")]
    public int Id{get;private set;}
    [Column("user_id")]
    public int UserId{get;set;}
    [Column("company_name")]
    public string CompanyName{get;set;}=string.Empty;
    [Column("company_description")]
    public string? CompanyDescription{get;set;}
    [Column("company_website")]
    public string? CompanyWebsite{get;set;}
    [Column("industry")]
    public string Industry{get;set;}=string.Empty;
    [Column("company_size")]
    public string? CompanySize{get;set;}
    [Column("headquarters")]
    public string? Headquarters{get;set;}
    [Column("company_logo")]
    public string? CompanyLogo{get;set;}
    [Column("verified")]
    public bool Verified{get;set;}=false;
    [Column("created_at")]
    public DateTime CreatedAt{get;private set;}=DateTime.UtcNow;
    [Column("verification_document_path")]
    public string? VerificationDocumentPath{get;set;}
    [Column("email_notification_enabled")]
    public bool EmailNotificationsEnabled{get;set;}=true;

    public virtual User User{get;set;}=null!;
    public virtual ICollection<Job> Jobs{get;set;}=[];

}