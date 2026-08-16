using System.ComponentModel.DataAnnotations;

namespace AucaJobPortalDotNet.DTOs;
public class RegisterEmployerDTO
{
    [Required, EmailAddress]
    public string Email{get;set;}=string.Empty;
    [Required, MinLength(8)]
    public string Password{get;set;}=string.Empty;
    [Required]
    public string Phone{get;set;}=string.Empty;
    [Required]
    public string CompanyName{get;set;}=string.Empty;
    [Required]
    public string CompanyDescription{get;set;}=string.Empty;
    public string? CompanyWebsite{get;set;}
    [Required]
    public string Industry{get;set;}=string.Empty;

    public string? CompanySize{get;set;}
    [Required]
    public string Location{get;set;}=string.Empty;
    [Required]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "TIN must be exactly 9 digits (numbers only)")]
    public string Tin{get;set;}=string.Empty;

}