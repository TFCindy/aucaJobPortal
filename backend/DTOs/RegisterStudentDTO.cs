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
    public string Phone{get;set;}=string.Empty;
    [Required, MinLength(8)]
    public string Password{get;set;}=string.Empty;
    [Required]
    public bool IsAlumni { get; set; } = false;
    public string? Program{get;set;}

    
    
}