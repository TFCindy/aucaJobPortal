using AucaJobPortalDotNet.Models;

namespace AucaJobPortalDotNet.Interfaces;

public interface IEmailService
{
    Task<bool> SendVerificationEmailAsync(string email, string fullName, string token);
    Task<bool> SendPasswordResetEmailAsync(string email, string fullName, string token);
    Task<bool> SendWelcomeEmailAsync(string email, string fullName, string userType = "student");
    Task<bool> SendApplicationStatusEmailAsync(string email, string fullName, string jobTitle, string status);
    Task<bool> SendApplicationNotificationsAsync(Application application, Job job, Student student, Employer employer);
}