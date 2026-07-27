namespace AucaJobPortalDotNet.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendVerificationEmailAsync(string email, string fullName, string token);
    Task<bool> SendPasswordResetEmailAsync(string email, string fullName, string token);
    Task<bool> SendWelcomeEmailAsync(string email, string fullName);
}