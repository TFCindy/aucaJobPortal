using AucaJobPortalDotNet.Interfaces;
using AucaJobPortalDotNet.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AucaJobPortalDotNet.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly AucaDbContext _context;

    public EmailService(IConfiguration configuration, AucaDbContext context)
    {
        _configuration = configuration;
        _context = context;
        
    }

    // ─── EXISTING METHOD: SendVerificationEmailAsync (UNCHANGED) ───
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

        return await SendEmailAsync(email, subject, body );
    }

    // ─── EXISTING METHOD: SendPasswordResetEmailAsync (UNCHANGED) ───
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

    public async Task<bool> SendWelcomeEmailAsync(string email, string fullName, string userType = "student")
    {
        var subject = $"Welcome to AUCA Job Portal, {fullName}!";
        string body;

        if (userType?.ToLower() == "employer")
        {
            // ─── EMPLOYER VERSION ───
            body = $@"
                <h2>Welcome to AUCA Job Portal, {fullName}!</h2>
                <p>Your employer account has been successfully verified.</p>
                
                <h3>Your Employer Dashboard:</h3>
                <ul>
                    <li>📝 Post and manage job listings</li>
                    <li>👥 Review and manage applications</li>
                    <li>📊 Track hiring analytics</li>
                    <li>📧 Communicate with applicants</li>
                </ul>

                <p><a href='{_configuration["FRONTEND_URL"] ?? "http://localhost:5173"}/employer/dashboard' 
                      style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                      Go to Dashboard
                </a></p>
            ";
        }
        else
        {
            // ─── STUDENT VERSION ───
            body = $@"
                <h2>Welcome to AUCA Job Portal, {fullName}!</h2>
                <p>Your student account has been successfully verified.</p>
                
                <h3>Your Student Dashboard:</h3>
                <ul>
                    <li>🔍 Browse and search jobs</li>
                    <li>📄 Apply with your CV</li>
                    <li>📊 Track applications</li>
                    <li>📱 Get job alerts</li>
                </ul>

                <p><a href='{_configuration["FRONTEND_URL"] ?? "http://localhost:5173"}/student/dashboard' 
                      style='background-color: #28a745; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                      Go to Dashboard
                </a></p>
            ";
        }

        return await SendEmailAsync(email, subject, body);
    }
    // ─── EXISTING METHOD: SendApplicationStatusEmailAsync (UNCHANGED) ───
    public async Task<bool> SendApplicationStatusEmailAsync(string email, string fullName, string jobTitle, string status)
    {
        var subject = $"Application Update: {jobTitle}";
        var statusMessage = status switch
        {
            "shortlisted" => "Great news! You've been shortlisted for this position.",
            "hired" => "Congratulations! You've been selected for this position.",
            "rejected" => "Thank you for your interest. Unfortunately, you were not selected for this position at this time.",
            "pending" => "Your application is currently under review.",
            _ => $"Your application status has been updated to: {status}"
        };

        var body = $@"
            <h2>Hi {fullName},</h2>
            <p>{statusMessage}</p>
            <p><strong>Position:</strong> {jobTitle}</p>
            <p>Log in to the AUCA Job Portal for more details.</p>
            <hr />
            <p style='color: #666; font-size: 12px;'>This is an automated notification from AUCA Job Portal.</p>
        ";

        return await SendEmailAsync(email, subject, body);
    }

    // ─── NEW: SendApplicationNotificationsAsync (FOR BOTH STUDENT AND EMPLOYER) ───
    public async Task<bool> SendApplicationNotificationsAsync(
        Application application, 
        Job job, 
        Student student, 
        Employer employer)
    {
        try
        {
            var studentUser = student.User;
            var employerUser = employer.User;

            // 1. Send confirmation to student
            var studentSubject = $"Application Submitted for {job.Title}";
            var studentBody = $@"
                <h2>Hi {studentUser.FullName},</h2>
                <p>Your application for <strong>{job.Title}</strong> has been submitted successfully!</p>
                
                <h3>Application Details:</h3>
                <ul>
                    <li><strong>Position:</strong> {job.Title}</li>
                    <li><strong>Company:</strong> {employer.CompanyName}</li>
                    <li><strong>Application ID:</strong> {application.Id}</li>
                    <li><strong>Date Applied:</strong> {DateTime.Now:MMMM dd, yyyy HH:mm}</li>
                    <li><strong>Status:</strong> Pending Review</li>
                </ul>

                <h3>What happens next?</h3>
                <ul>
                    <li>The employer will review your application</li>
                    <li>You'll receive email updates when your status changes</li>
                    <li>Check your application status anytime in your dashboard</li>
                </ul>

                <p><a href='{_configuration["FRONTEND_URL"] ?? "http://localhost:5173"}/student/applications' 
                      style='background-color: #28a745; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                      Track Your Applications
                </a></p>

                <hr />
                <p style='color: #666; font-size: 12px;'>Thank you for applying through AUCA Job Portal. Good luck with your application!</p>
            ";

            await SendEmailAsync(studentUser.Email, studentSubject, studentBody);

            // 2. Send notification to employer (only if enabled)
            if (employer.EmailNotificationsEnabled)
            {
                var employerSubject = $"New Application: {studentUser.FullName} for {job.Title}";
                var employerBody = $@"
                    <h2>New Application Received</h2>
                    <p><strong>Company:</strong> {employer.CompanyName}</p>
                    <p><strong>Position:</strong> {job.Title}</p>
                    
                    <h3>Applicant Information:</h3>
                    <ul>
                        <li><strong>Name:</strong> {studentUser.FullName}</li>
                        <li><strong>Email:</strong> {studentUser.Email}</li>
                        <li><strong>Phone:</strong> {studentUser.Phone ?? "Not provided"}</li>
                        <li><strong>Student ID:</strong> {student.StudentId}</li>
                        <li><strong>Program:</strong> {student.Program}</li>
                        <li><strong>Year of Study:</strong> {student.YearOfStudy?.ToString() ?? "Not specified"}</li>
                        <li><strong>Faculty:</strong> {student.Faculty ?? "Not specified"}</li>
                        <li><strong>Department:</strong> {student.Department ?? "Not specified"}</li>
                    </ul>

                    <h3>Application Materials:</h3>
                    <ul>
                        <li><strong>Cover Letter:</strong> {(string.IsNullOrEmpty(application.CoverLetter) ? "Not provided" : "Provided ✓")}</li>
                        <li><strong>CV/Resume:</strong> {(string.IsNullOrEmpty(application.CvFilePath) ? "Not uploaded" : "Uploaded ✓")}</li>
                        <li><strong>Professional Experience:</strong> {(string.IsNullOrEmpty(application.ProfessionalExperience) ? "Not provided" : "Provided ✓")}</li>
                        <li><strong>Skills:</strong> {application.Skills ?? "Not specified"}</li>
                    </ul>

                    <h3>Quick Actions:</h3>
                    <ul>
                        <li><a href='{_configuration["FRONTEND_URL"] ?? "http://localhost:5173"}/employer/applications/{application.Id}'>View Full Application</a></li>
                        <li><a href='{_configuration["FRONTEND_URL"] ?? "http://localhost:5173"}/employer/applications/{application.Id}/status'>Update Application Status</a></li>
                        <li><a href='{_configuration["FRONTEND_URL"] ?? "http://localhost:5173"}/employer/messages/new?studentId={student.Id}'>Send Message to Applicant</a></li>
                    </ul>

                    <br />
                    <a href='{_configuration["FRONTEND_URL"] ?? "http://localhost:5173"}/employer/applications' 
                       style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                       View All Applications
                    </a>

                    <hr />
                    <p style='color: #666; font-size: 12px;'>This is an automated notification from AUCA Job Portal.</p>
                    <p style='color: #666; font-size: 12px;'>
                        You received this email because you have email notifications enabled. 
                        You can change this in your <a href='{_configuration["FRONTEND_URL"] ?? "http://localhost:5173"}/employer/settings'>employer settings</a>.
                    </p>
                ";

                await SendEmailAsync(employerUser.Email, employerSubject, employerBody);
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }



    // ─── PRIVATE: SendEmailAsync (UNCHANGED) ───
    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body, string emailType = "General", int? userId = null)
{
    // 1. CREATE LOG
    var emailLog = new EmailLog
    {
        RecipientEmail = toEmail,
        EmailType = emailType,
        Subject = subject,
        Status = "Pending",
        SentAt = DateTime.UtcNow,
        UserId = userId
    };

    // 2.SAVE LOG IMMEDIATELY (BEFORE sending email)
    _context.EmailLogs.Add(emailLog);
    await _context.SaveChangesAsync();

    try
    {
        var emailSettings = _configuration.GetSection("EmailSettings");
        var host = emailSettings["Host"] ?? "smtp.gmail.com";
        var port = int.Parse(emailSettings["Port"] ?? "587");
        var username = emailSettings["Username"] ?? "";
        var password = emailSettings["Password"] ?? "";
        var fromName = emailSettings["FromName"] ?? "AUCA Job Portal";

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            emailLog.Status = "Failed";
            emailLog.ErrorMessage = "Email credentials not configured";
            await _context.SaveChangesAsync();
            
            return false;
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, username));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = body };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new MailKit.Net.Smtp.SmtpClient();
        client.CheckCertificateRevocation = false;

        await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(username, password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        // 4. UPDATE LOG ON SUCCESS
        emailLog.Status = "Sent";
        await _context.SaveChangesAsync();
        
        return true;
    }
    catch (Exception ex)
    {
        emailLog.Status = "Failed";
        emailLog.ErrorMessage = ex.Message;
        await _context.SaveChangesAsync();
        
        return false;
    }
}

}