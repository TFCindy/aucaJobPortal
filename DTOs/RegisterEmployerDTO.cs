using System.ComponentModel.DataAnnotations;

namespace AucaJobPortalDotNet.DTOs;
public class RegisterEmployerDTO
{
    [Required, EmailAddress]
    public string Email{get;set;}=string.Empty;
    [Required, MinLength(8)]
    public string Password{get;set;}=string.Empty;
    public string? Phone{get;set;}
    [Required]
    public string CompanyName{get;set;}=string.Empty;
    public string? CompanyDescription{get;set;}
    public string? CompanyWebsite{get;set;}
    [Required]
    public string Industry{get;set;}=string.Empty;

    public string? CompanySize{get;set;}
    public string? HeadQuarters{get;set;}
    [Required]
    public string FullName{get;set;}=string.Empty;

}