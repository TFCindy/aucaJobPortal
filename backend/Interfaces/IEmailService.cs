using AucaJobPortalDotNet.Models;

namespace AucaJobPortalDotNet.Interfaces;

public interface IEmailService
{
    Task<bool> SendVerificationEmailAsync(string email, string fullName, string token);
    Task<bool> SendPasswordResetEmailAsync(string email, string fullName, string token);
    Task<bool> SendWelcomeEmailAsync(string email, string fullName, string userType = "student");
    Task<bool> SendApplicationStatusEmailAsync(string email, string fullName, string jobTitle, string status);
    Task<bool> SendProgramApplicationStatusEmailAsync(string email, string fullName, string programTitle, string status);
    Task<bool> SendApplicationNotificationsAsync(Application application, Job job, Student student, Employer employer);
    Task<bool> SendProgramApplicationSubmittedEmailAsync(string counselorEmail, string counselorName, string studentName, string programTitle, int applicationId);
    Task<bool> SendProgramApplicationConfirmationEmailAsync(string studentEmail, string studentName, string programTitle, int applicationId);
}