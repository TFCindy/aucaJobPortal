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
            EmailNotificationsEnabled = student.EmailNotificationsEnabled
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

        // Update User fields
        if (!string.IsNullOrEmpty(dto.Phone))
            student.User!.Phone = dto.Phone;
            

        student.User!.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetStudentProfileAsync(userId);
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
            CompanySize = employer.CompanySize,
            Headquarters = employer.Headquarters,
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
        
        if (!string.IsNullOrEmpty(dto.Headquarters))
            employer.Headquarters = dto.Headquarters;

        // Update User fields
        if (!string.IsNullOrEmpty(dto.Phone))
            employer.User!.Phone = dto.Phone;

        employer.User!.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetEmployerProfileAsync(userId);
    }
}