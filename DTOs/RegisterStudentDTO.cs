using System.ComponentModel.DataAnnotations;

namespace AucaJobPortalDotNet.DTOs;
public class RegisterStudentDTO
{
   [Required(AllowEmptyStrings = false)]
    public string StudentId{get;set;}=string.Empty;
    [Required, EmailAddress]
    public string Email{get;set;}=string.Empty;
    [Required]
    public string FullName{get;set;}=string.Empty;
    [Required]
    public string Program{get;set;}=string.Empty;
    public string? Faculty{get;set;}
    public string? Department{get;set;}
    public string? Phone{get;set;}
    [Required, MinLength(8)]
    public string Password{get;set;}=string.Empty;
    public int? YearOfStudy{get;set;}
    public int? GraduationYear{get;set;}
}