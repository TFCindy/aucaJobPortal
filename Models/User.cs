using System.Data.SqlTypes;
using Microsoft.AspNetCore.Identity;

namespace AucaJobPortalDotNet.Models;
public class User
{
    public int Id{get;private set;}
    public string Email{get;set;}=string.Empty;
    public string PasswordHash{get;set;}=string.Empty;
    public string FullName{get;set;}=string.Empty;
    public string? Phone{get;set;}
    public bool IsActive{get;set;}=true;
    public DateTime CreatedAt{get;private set;}=DateTime.UtcNow;
    public DateTime UpdatedAt{get;set;}=DateTime.UtcNow;//// Returns the current UTC time (no timezone offset)
    public DateTime? LastLoginAt{get;set;}=DateTime.UtcNow;//datetime.now // Returns the current time on YOUR computer (in your timezone)
    public string? PasswordResetToken{get;set;}
    public DateTime? PasswordResetExpires{set;get;}
    public bool EmailVerified{get;set;}=false;
    public string? EmailVerificationToken{get;set;}//can be null when the user email has already been verified or the token has expired
    public DateTime? EmailVerifificationExpires{ get; set; } //can be null when No active token (email already verified)
    public int RoleId{get;set;}
    public virtual Role Role{get;set;}=null!;
    public Student? Student{get;set;}

    public Employer? Employer{get;set;}




}