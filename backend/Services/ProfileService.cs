using Microsoft.EntityFrameworkCore;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Models;
using AucaJobPortalDotNet.Services.Interfaces;

namespace AucaJobPortalDotNet.Services;

public class ProfileService : IProfileService
{
    private readonly AucaDbContext _context;

    public ProfileService(AucaDbContext context)
    {
        _context = context;
    }

    // ─── GET STUDENT PROFILE ───
    public async Task<StudentProfileDTO> GetStudentProfileAsync(int userId)
    {
        var student = await _context.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            throw new Exception("Student profile not found");
        }

        return new StudentProfileDTO
        {
            Id = student.Id,
            UserId = student.UserId,
            StudentId = student.StudentId,
            Program = student.Program,
            Faculty = student.Faculty,
            Department = student.Department,
            YearOfStudy = student.YearOfStudy,
            GraduationYear = student.GraduationYear,
            Phone = student.User?.Phone,
            Skills = student.Skills,
            Bio = student.Bio,
            ProfilePicture = student.ProfilePicture,
            CvFilePath = student.CvFilePath,
            EmailNotificationsEnabled = student.EmailNotificationsEnabled,
            FullName = student.User?.FullName ?? string.Empty,
            Email = student.User?.Email ?? string.Empty,
            IsAlumni = student.IsAlumni,
            DegreeClassification = student.DegreeClassification,
            CurrentEmploymentStatus = student.CurrentEmploymentStatus,
            CurrentEmployer = student.CurrentEmployer,
            CurrentJobTitle = student.CurrentJobTitle,
            Industry = student.Industry,
            EmploymentStartDate = student.EmploymentStartDate,
            TranscriptFilePath = student.TranscriptFilePath,
            DiplomaFilePath = student.DiplomaFilePath
        };
    }

    // ─── UPDATE STUDENT PROFILE ───
    public async Task<StudentProfileDTO> UpdateStudentProfileAsync(int userId, UpdateStudentProfileDTO dto)
{
    var student = await _context.Students
        .Include(s => s.User)
        .FirstOrDefaultAsync(s => s.UserId == userId);

    if (student == null)
    {
        throw new Exception("Student profile not found");
    }

    // Update Student fields
    if (!string.IsNullOrEmpty(dto.Program))
        student.Program = dto.Program;
    
    if (!string.IsNullOrEmpty(dto.Faculty))
        student.Faculty = dto.Faculty;
    
    if (!string.IsNullOrEmpty(dto.Department))
        student.Department = dto.Department;
    
    if (dto.YearOfStudy.HasValue)
        student.YearOfStudy = dto.YearOfStudy;
    
    if (dto.GraduationYear.HasValue)
        student.GraduationYear = dto.GraduationYear;
    
    if (!string.IsNullOrEmpty(dto.Skills))
        student.Skills = dto.Skills;
    
    if (!string.IsNullOrEmpty(dto.Bio))
        student.Bio = dto.Bio;

    if (!string.IsNullOrEmpty(dto.Phone))
        student.User!.Phone = dto.Phone;

    if (dto.ProfilePicture != null && dto.ProfilePicture.Length > 0)
    {
        var picturePath = await SaveFileAsync(dto.ProfilePicture, "profiles", $"profile_{student.Id}");
        student.ProfilePicture = picturePath;
    }

    if (dto.CvFile != null && dto.CvFile.Length > 0)
    {
        var cvPath = await SaveFileAsync(dto.CvFile, "cvs", $"cv_{student.Id}");
        student.CvFilePath = cvPath;
    }

    if (dto.GraduationYear.HasValue)
    student.GraduationYear = dto.GraduationYear;

   if (!string.IsNullOrEmpty(dto.DegreeClassification))
    student.DegreeClassification = dto.DegreeClassification;

   if (!string.IsNullOrEmpty(dto.CurrentEmploymentStatus))
    student.CurrentEmploymentStatus = dto.CurrentEmploymentStatus;

   if (!string.IsNullOrEmpty(dto.CurrentEmployer))
    student.CurrentEmployer = dto.CurrentEmployer;

   if (!string.IsNullOrEmpty(dto.CurrentJobTitle))
    student.CurrentJobTitle = dto.CurrentJobTitle;

   if (!string.IsNullOrEmpty(dto.Industry))
    student.Industry = dto.Industry;

   if (dto.EmploymentStartDate.HasValue)
    student.EmploymentStartDate = dto.EmploymentStartDate;
    if (dto.TranscriptFile != null && dto.TranscriptFile.Length > 0)
{
    var transcriptPath = await SaveFileAsync(dto.TranscriptFile, "transcripts", $"transcript_{student.Id}");
    student.TranscriptFilePath = transcriptPath;
}

   if (dto.DiplomaFile != null && dto.DiplomaFile.Length > 0)
{
    // Only allow if alumni
    if (student.IsAlumni)
    {
        var diplomaPath = await SaveFileAsync(dto.DiplomaFile, "diplomas", $"diploma_{student.Id}");
        student.DiplomaFilePath = diplomaPath;
    }
    else
    {
        throw new Exception("Only alumni can upload a diploma certificate");
    }
}

    student.User!.UpdatedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();

    return await GetStudentProfileAsync(userId);
}

// ─── ADD THIS HELPER METHOD ───
private async Task<string> SaveFileAsync(IFormFile file, string subFolder, string prefix)
{
    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", subFolder);
    if (!Directory.Exists(uploadsFolder))
    {
        Directory.CreateDirectory(uploadsFolder);
    }

    var extension = Path.GetExtension(file.FileName);
    var fileName = $"{prefix}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}{extension}";
    var filePath = Path.Combine(uploadsFolder, fileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    return $"/uploads/{subFolder}/{fileName}";
}

    // ─── GET EMPLOYER PROFILE ───
    public async Task<EmployerProfileDTO> GetEmployerProfileAsync(int userId)
    {
        var employer = await _context.Employers
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.UserId == userId);

        if (employer == null)
        {
            throw new Exception("Employer profile not found");
        }

        return new EmployerProfileDTO
        {
            Id = employer.Id,
            UserId = employer.UserId,
            CompanyName = employer.CompanyName,
            CompanyDescription = employer.CompanyDescription,
            CompanyWebsite = employer.CompanyWebsite,
            Industry = employer.Industry,
            Email = employer.User?.Email ?? string.Empty,
            CompanySize = employer.CompanySize,
            Location = employer.Location,
            CompanyLogo = employer.CompanyLogo,
            Verified = employer.Verified,
            EmailNotificationsEnabled = employer.EmailNotificationsEnabled,
            Phone = employer.User?.Phone
        };
    }

    // ─── UPDATE EMPLOYER PROFILE ───
    public async Task<EmployerProfileDTO> UpdateEmployerProfileAsync(int userId, UpdateEmployerProfileDTO dto)
    {
        var employer = await _context.Employers
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.UserId == userId);

        if (employer == null)
        {
            throw new Exception("Employer profile not found");
        }

        // Update Employer fields
        if (!string.IsNullOrEmpty(dto.CompanyName))
            employer.CompanyName = dto.CompanyName;
        
        if (!string.IsNullOrEmpty(dto.CompanyDescription))
            employer.CompanyDescription = dto.CompanyDescription;
        
        if (!string.IsNullOrEmpty(dto.CompanyWebsite))
            employer.CompanyWebsite = dto.CompanyWebsite;
        
        if (!string.IsNullOrEmpty(dto.Industry))
            employer.Industry = dto.Industry;
        
        if (!string.IsNullOrEmpty(dto.CompanySize))
            employer.CompanySize = dto.CompanySize;
        
        if (!string.IsNullOrEmpty(dto.Location))
            employer.Location = dto.Location;

        if (dto.CompanyLogo != null && dto.CompanyLogo.Length > 0)
        {
            if (!string.IsNullOrEmpty(employer.CompanyLogo))
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", employer.CompanyLogo.TrimStart('/'));
                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }
            }

            employer.CompanyLogo = await SaveFileAsync(dto.CompanyLogo, "logos", $"logo_{employer.Id}");
        }

        // Update User fields
        if (!string.IsNullOrEmpty(dto.Phone))
            employer.User!.Phone = dto.Phone;

        employer.User!.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetEmployerProfileAsync(userId);
    }

   public async Task<AlumniProfileDTO> GetAlumniProfileAsync(int userId)
    {
        var student = await _context.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            throw new Exception("Student profile not found");
        }

        // Check if student is actually alumni
        if (!student.IsAlumni)
        {
            throw new Exception("Student is not an alumni. Please use GetStudentProfileAsync.");
        }

        return new AlumniProfileDTO
        {
            // Basic Info
            Id = student.Id,
            UserId = student.UserId,
            StudentId = student.StudentId,
            FullName = student.User?.FullName ?? string.Empty,
            Email = student.User?.Email ?? string.Empty,
            Phone = student.User?.Phone,
            ProfilePicture = student.ProfilePicture,
            Bio = student.Bio,
            Skills = student.Skills,
            CvFilePath = student.CvFilePath,
            EmailNotificationsEnabled = student.EmailNotificationsEnabled,

            // Academic Info
            Program = student.Program,
            Faculty = student.Faculty,
            Department = student.Department,
            GraduationYear = student.GraduationYear ?? DateTime.UtcNow.Year,
            DegreeClassification = student.DegreeClassification ?? string.Empty,

            // Employment Info
            CurrentEmploymentStatus = student.CurrentEmploymentStatus,
            CurrentEmployer = student.CurrentEmployer,
            CurrentJobTitle = student.CurrentJobTitle,
            Industry = student.Industry,
            EmploymentStartDate = student.EmploymentStartDate,

            // Document Verification Status
            TranscriptVerified = student.TranscriptVerified,
            DiplomaVerified = student.DiplomaVerified,
            DocumentVerificationStatus = student.DocumentVerificationStatus,

            // Timestamps
            CreatedAt = student.CreatedAt,
            AlumniSince = student.AlumniSince
        };
    }

    // ─── UPDATE ALUMNI PROFILE ───
    public async Task<AlumniProfileDTO> UpdateAlumniProfileAsync(int userId, UpdateAlumniProfileDTO dto)
    {
        var student = await _context.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            throw new Exception("Student profile not found");
        }

        // Check if student is actually alumni
        if (!student.IsAlumni)
        {
            throw new Exception("Student is not an alumni. Please use UpdateStudentProfileAsync.");
        }

        // ─── UPDATE ACADEMIC INFO ───
        if (!string.IsNullOrEmpty(dto.Program))
            student.Program = dto.Program;

        if (!string.IsNullOrEmpty(dto.Faculty))
            student.Faculty = dto.Faculty;

        if (!string.IsNullOrEmpty(dto.Department))
            student.Department = dto.Department;

        if (dto.GraduationYear.HasValue)
            student.GraduationYear = dto.GraduationYear;

        if (!string.IsNullOrEmpty(dto.DegreeClassification))
            student.DegreeClassification = dto.DegreeClassification;

        // ─── UPDATE EMPLOYMENT INFO ───
        if (!string.IsNullOrEmpty(dto.CurrentEmploymentStatus))
            student.CurrentEmploymentStatus = dto.CurrentEmploymentStatus;

        if (!string.IsNullOrEmpty(dto.CurrentEmployer))
            student.CurrentEmployer = dto.CurrentEmployer;

        if (!string.IsNullOrEmpty(dto.CurrentJobTitle))
            student.CurrentJobTitle = dto.CurrentJobTitle;

        if (!string.IsNullOrEmpty(dto.Industry))
            student.Industry = dto.Industry;

        if (dto.EmploymentStartDate.HasValue)
            student.EmploymentStartDate = dto.EmploymentStartDate;

        // ─── UPDATE PERSONAL INFO ───
        if (!string.IsNullOrEmpty(dto.Bio))
            student.Bio = dto.Bio;

        if (!string.IsNullOrEmpty(dto.Skills))
            student.Skills = dto.Skills;

        if (!string.IsNullOrEmpty(dto.ProfilePicture))
            student.ProfilePicture = dto.ProfilePicture;

        if (!string.IsNullOrEmpty(dto.CvFilePath))
            student.CvFilePath = dto.CvFilePath;

        if (dto.EmailNotificationsEnabled.HasValue)
            student.EmailNotificationsEnabled = dto.EmailNotificationsEnabled.Value;

        // ─── UPDATE USER FIELDS ───
        if (!string.IsNullOrEmpty(dto.Phone))
            student.User!.Phone = dto.Phone;

        student.User!.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetAlumniProfileAsync(userId);
    }

    public async Task<CareerCounselorProfileDTO> GetCareerCounselorProfileAsync(int userId)
    {
        var counselor = await _context.CareerCounselors
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (counselor == null)
        {
            throw new Exception("Career counselor profile not found");
        }

        return new CareerCounselorProfileDTO
        {
            Id = counselor.Id,
            UserId = counselor.UserId,
            Department = counselor.Department,
            Title = counselor.Title,
            Bio = counselor.Bio,
            Specialization = counselor.Specialization,
            YearsOfExperience = counselor.YearsOfExperience,
            OfficeLocation = counselor.OfficeLocation,
            IsActive = counselor.IsActive,
            FullName = counselor.User?.FullName ?? string.Empty,
            Email = counselor.User?.Email ?? string.Empty,
            Phone = counselor.User?.Phone
        };
    }

    public async Task<CareerCounselorProfileDTO> UpdateCareerCounselorProfileAsync(int userId, UpdateCareerCounselorProfileDTO dto)
    {
        var counselor = await _context.CareerCounselors
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (counselor == null)
        {
            throw new Exception("Career counselor profile not found");
        }

        if (!string.IsNullOrEmpty(dto.Department))
            counselor.Department = dto.Department;

        if (!string.IsNullOrEmpty(dto.Title))
            counselor.Title = dto.Title;

        if (dto.Bio != null)
            counselor.Bio = dto.Bio;

        if (dto.Specialization != null)
            counselor.Specialization = dto.Specialization;

        if (dto.YearsOfExperience.HasValue)
            counselor.YearsOfExperience = dto.YearsOfExperience;

        if (dto.OfficeLocation != null)
            counselor.OfficeLocation = dto.OfficeLocation;

        if (!string.IsNullOrEmpty(dto.Phone))
            counselor.User!.Phone = dto.Phone;

        counselor.UpdatedAt = DateTime.UtcNow;
        counselor.User!.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetCareerCounselorProfileAsync(userId);
    }
}