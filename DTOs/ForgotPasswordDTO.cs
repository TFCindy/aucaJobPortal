using System.ComponentModel.DataAnnotations;

namespace AucaJobPortalDotNet.DTOs;
public class ForgotPasswordDTO
{
    [Required]
    [EmailAddress]
    public string Email{get;set;}=string.Empty;

}