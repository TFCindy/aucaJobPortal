namespace AucaJobPortalDotNet.DTOs;
public class RegisterStudentDTO
{
    public string StudentId{get;}=string.Empty;
    public string Email{get;set;}=string.Empty;
    public string FullName{get;set;}=string.Empty;
    public string Program{get;set;}=string.Empty;
    public string? Faculty{get;set;}
    public string? Department{get;set;}
    public string? Phone{get;set;}
    public string Password{get;set;}=string.Empty;
    public string? YearOfStudy{get;set;}
    public string? GraduationYear{get;set;}
}