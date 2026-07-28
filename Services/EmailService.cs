using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using AucaJobPortalDotNet.Interfaces;


namespace AucaJobPortalDotNet.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> SendVerificationEmailAsync(string email, string fullName, string token)
    {
        var subject = "Verify Your Email - AUCA Job Portal";
        var frontendUrl = _configuration["FRONTEND_URL"] ?? "http://localhost:5173";
        var body = $@"
            <h2>Welcome to AUCA Job Portal, {fullName}!</h2>
            <p>Please verify your email by clicking the link below:</p>
            <a href='{frontendUrl}/verify-email?token={token}'>
                Verify Email
            </a>
            <p>This link expires in 24 hours.</p>
            <p>If you didn't register, please ignore this email.</p>
        ";

        return await SendEmailAsync(email, subject, body);
    }

    public async Task<bool> SendPasswordResetEmailAsync(string email, string fullName, string token)
    {
        var subject = "Reset Your Password - AUCA Job Portal";
        var frontendUrl = _configuration["FRONTEND_URL"] ?? "http://localhost:5173";
        var body = $@"
            <h2>Password Reset Request</h2>
            <p>Hi {fullName},</p>
            <p>Click the link below to reset your password:</p>
            <a href='{frontendUrl}/reset-password?token={token}'>
                Reset Password
            </a>
            <p>This link expires in 1 hour.</p>
            <p>If you didn't request this, please ignore this email.</p>
        ";

        return await SendEmailAsync(email, subject, body);
    }

    public async Task<bool> SendWelcomeEmailAsync(string email, string fullName)
    {
        var subject = "Welcome to AUCA Job Portal!";
        var body = $@"
            <h2>Welcome to AUCA Job Portal, {fullName}!</h2>
            <p>Your account has been successfully verified.</p>
            <p>You can now log in and start using our platform.</p>
            <p>Explore job opportunities and connect with employers!</p>
        ";

        return await SendEmailAsync(email, subject, body);
    }

    private async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            // ─── READ FROM CONFIGURATION ───
            var emailSettings = _configuration.GetSection("EmailSettings");
            var host = emailSettings["Host"] ?? "smtp.gmail.com";
            var port = int.Parse(emailSettings["Port"] ?? "587");
            var username = emailSettings["Username"] ?? "";
            var password = emailSettings["Password"] ?? "";
            var fromName = emailSettings["FromName"] ?? "AUCA Job Portal";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Email credentials not configured. Skipping email send.");
                return false;
            }

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(username, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email sending failed: {ex.Message}");
            return false;
        }
    }
}