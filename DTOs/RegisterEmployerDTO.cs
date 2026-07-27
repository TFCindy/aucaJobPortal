namespace AucaJobPortalDotNet.DTOs;
public class RegisterEmplyerDTO
{
    public string Emai{get;set;}=string.Empty;
    public string Password{get;set;}=string.Empty;
    public string? Phone{get;set;}
    public string CompanyName{get;set;}=string.Empty;
    public string? CompanyDescription{get;set;}
    public string? CompanyWebsite{get;set;}
    public string Industry{get;set;}=string.Empty;

    public string? CompanySize{get;set;}
    public string? HeadQuarters{get;set;}
    public string FullName{get;set;}=string.Empty;

}