using Microsoft.EntityFrameworkCore;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Models;
using AucaJobPortalDotNet.Services.Interfaces;
using AucaJobPortalDotNet.Interfaces;

namespace AucaJobPortalDotNet.Services;

public class ApplicationService : IApplicationService
{
    private readonly AucaDbContext _context;
    private readonly INotificationService _notificationService;
    private readonly IEmailService _emailService;

    public ApplicationService(
        AucaDbContext context,
        INotificationService notificationService,
        IEmailService emailService)
    {
        _context = context;
        _notificationService = notificationService;
        _emailService = emailService;
    }

    private IQueryable<Application> ApplicationsWithIncludes()
    {
        return _context.Applications
            .Include(a => a.Job)
                .ThenInclude(j => j.Employer)
                    .ThenInclude(e => e.User)
            .Include(a => a.Student)
                .ThenInclude(s => s.User);
    }

    // ─── APPLY FOR A JOB ───
   public async Task<ApplicationResponseDTO> ApplyAsync(int userId, CreateApplicationDTO dto)
{
    var job = await _context.Jobs
        .Include(j => j.Employer)
            .ThenInclude(e => e.User)
        .FirstOrDefaultAsync(j => j.Id == dto.JobId && j.Status == "active");

    if (job == null)
    {
        throw new Exception("Job not found or not active");
    }

    var student = await _context.Students
        .Include(s => s.User)
        .FirstOrDefaultAsync(s => s.UserId == userId);

    if (student == null)
    {
        throw new Exception("Student profile not found");
    }

    var existingApplication = await _context.Applications
        .FirstOrDefaultAsync(a => a.JobId == dto.JobId && a.StudentId == student.Id);

    if (existingApplication != null)
    {
        throw new Exception("You have already applied to this job");
    }

    using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        var application = new Application
        {
            JobId = dto.JobId,
            StudentId = student.Id,
            CoverLetter = dto.CoverLetter,
            Status = "pending",
            UpdatedAt = DateTime.UtcNow,
            ProfessionalExperience = dto.ProfessionalExperience,
            Skills = dto.Skills,
            EducationLevel = dto.EducationLevel,
            EducationDescription = dto.EducationDescription
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            userId: job.Employer.UserId,
            title: "New Application Received",
            message: $"A new application was submitted for {job.Title}",
            type: "new_application",
            entityType: "application",
            entityId: application.Id
        );

        await transaction.CommitAsync();

        await _emailService.SendApplicationNotificationsAsync(application, job, student, job.Employer);

        return await GetApplicationByIdAsync(application.Id);
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}

    // ─── GET MY APPLICATIONS ───
    public async Task<List<ApplicationResponseDTO>> GetMyApplicationsAsync(int userId)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            throw new Exception("Student profile not found");
        }

        var applications = await ApplicationsWithIncludes()
            .Where(a => a.StudentId == student.Id)
            .OrderByDescending(a => a.AppliedAt)
            .ToListAsync();

        return applications.Select(MapToResponse).ToList();
    }

    // ─── GET APPLICATION BY ID ───
    public async Task<ApplicationResponseDTO> GetApplicationByIdAsync(int applicationId)
    {
        var application = await ApplicationsWithIncludes()
            .FirstOrDefaultAsync(a => a.Id == applicationId);

        if (application == null)
        {
            throw new Exception("Application not found");
        }

        return MapToResponse(application);
    }

    // ─── WITHDRAW APPLICATION ───
    public async Task<bool> WithdrawApplicationAsync(int applicationId, int userId)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            throw new Exception("Student profile not found");
        }

        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.Id == applicationId && a.StudentId == student.Id);

        if (application == null)
        {
            throw new Exception("Application not found");
        }

        if (application.Status != "pending")
        {
            throw new Exception("Cannot withdraw an application that has been reviewed");
        }

        application.Status = "withdrawn";
        application.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    // ─── GET APPLICATIONS FOR A JOB (Employer) ───
    public async Task<List<ApplicationResponseDTO>> GetApplicationsForJobAsync(int jobId, int employerId)
    {
        var job = await _context.Jobs
            .FirstOrDefaultAsync(j => j.Id == jobId && j.EmployerId == employerId);

        if (job == null)
        {
            throw new Exception("Job not found or you don't have permission");
        }

        var applications = await ApplicationsWithIncludes()
            .Where(a => a.JobId == jobId)
            .OrderByDescending(a => a.AppliedAt)
            .ToListAsync();

        return applications.Select(MapToResponse).ToList();
    }

    // ─── UPDATE APPLICATION STATUS (Employer) ───
    public async Task<ApplicationResponseDTO> UpdateApplicationStatusAsync(
    int applicationId,
    int employerId,
    UpdateApplicationStatusDTO dto)
{
    var application = await ApplicationsWithIncludes()
        .FirstOrDefaultAsync(a => a.Id == applicationId);

    if (application == null)
    {
        throw new Exception("Application not found");
    }

    if (application.Job.EmployerId != employerId)
    {
        throw new Exception("You don't have permission to update this application");
    }

    var validStatuses = new[] { "pending", "reviewed", "shortlisted", "rejected", "hired", "withdrawn" };
    if (!validStatuses.Contains(dto.Status))
    {
        throw new Exception($"Invalid status. Valid statuses: {string.Join(", ", validStatuses)}");
    }

    if (application.Status == "hired" || application.Status == "rejected" || application.Status == "withdrawn")
    {
        throw new Exception("Cannot change status of a finalised application");
    }

    using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        application.Status = dto.Status;
        application.EmployerFeedback = dto.EmployerFeedback;
        application.InterviewDate = dto.InterviewDate;
        application.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            userId: application.Student.UserId,
            title: "Application Status Updated",
            message: $"Your application for {application.Job.Title} is now: {dto.Status}",
            type: "application_status_update",
            entityType: "application",
            entityId: application.Id
        );

        await transaction.CommitAsync();

        // Email sent after commit, same reasoning as above
        if (dto.Status is "shortlisted" or "rejected" or "hired")
        {
            await _emailService.SendApplicationStatusEmailAsync(
                application.Student.User.Email,
                application.Student.User.FullName,
                application.Job.Title,
                dto.Status
            );
        }

        return await GetApplicationByIdAsync(applicationId);
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}
    // MAP TO RESPONSE 
    private ApplicationResponseDTO MapToResponse(Application application)
    {
        return new ApplicationResponseDTO
        {
            Id = application.Id,
            JobId = application.JobId,
            JobTitle = application.Job?.Title ?? "Unknown Job",
            CompanyName = application.Job?.Employer?.CompanyName ?? "Unknown Company",
            StudentName = application.Student?.User?.FullName ?? "Unknown Student",
            StudentIdNumber = application.Student?.Id ?? 0,  
            StudentEmail = application.Student?.User?.Email ?? "N/A",  
            StudentPhone = application.Student?.User?.Phone ?? "N/A",  
            Program = application.Student?.Program ?? "N/A", 
            YearOfStudy = application.Student?.YearOfStudy,  
            CoverLetter = application.CoverLetter,
            Status = application.Status,
            AppliedAt = application.AppliedAt,
            UpdatedAt = application.UpdatedAt,
            EmployerFeedback = application.EmployerFeedback,
            InterviewDate = application.InterviewDate,
            CvFilePath = application.CvFilePath,
            ProfessionalExperience = application.ProfessionalExperience,
            Skills = application.Skills,
            EducationLevel = application.EducationLevel,
            EducationDescription = application.EducationDescription
        };
    }

    public async Task<IEnumerable<ApplicationResponseDTO>> GetEmployerApplicationsAsync(int employerId)
{
    var applications = await _context.Applications
        .Include(a => a.Job)
        .Include(a => a.Student)
            .ThenInclude(s => s.User)
        .Where(a => a.Job.EmployerId == employerId)
        .OrderByDescending(a => a.AppliedAt)
        .Select(a => new ApplicationResponseDTO
        {
            Id = a.Id,
            JobId = a.JobId,
            JobTitle = a.Job.Title,
            StudentIdNumber = a.StudentId,
            StudentName = a.Student.User.FullName,
            StudentEmail = a.Student.User.Email,
            AppliedAt = a.AppliedAt,
            Status = a.Status,
            CoverLetter = a.CoverLetter,
            CvFilePath = a.CvFilePath
        })
        .ToListAsync();
    
    return applications;
}
}