using Microsoft.EntityFrameworkCore;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;
using AucaJobPortalDotNet.Models;

namespace AucaJobPortalDotNet.Services;

public class CareerProgramService : ICareerProgramService
{
    private readonly AucaDbContext _context;

    public CareerProgramService(AucaDbContext context)
    {
        _context = context;
    }

   
    public async Task<CareerProgramResponseDTO> CreateProgramAsync(int counselorId, CreateCareerProgramDTO dto)
    {
        var counselor = await _context.CareerCounselors
            .FirstOrDefaultAsync(c => c.UserId == counselorId);

        if (counselor == null)
        {
            throw new Exception("Career counselor not found");
        }

        if (dto.StartDate <= DateTime.UtcNow)
            throw new Exception("Start date must be in the future");

        if (dto.EndDate <= dto.StartDate)
            throw new Exception("End date must be after start date");

        if (dto.RegistrationDeadline >= dto.StartDate)
            throw new Exception("Registration deadline must be before start date");

        if (dto.MaxParticipants < 1)
            throw new Exception("Max participants must be at least 1");

        var program = new CareerProgram
        {
            CareerCounselorId = counselor.Id,
            Title = dto.Title,
            Description = dto.Description,
            ShortDescription = dto.ShortDescription,
            IsExternal = dto.IsExternal,
            ExternalOrganization = dto.ExternalOrganization,
            Type = dto.Type,    
            Level = dto.Level,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            RegistrationDeadline = dto.RegistrationDeadline,
            IsOnline = dto.IsOnline,
            Location = dto.Location,
            Venue = dto.Venue,
            MeetingLink = dto.MeetingLink,
            MaxParticipants = dto.MaxParticipants,
            IsFree = dto.IsFree,
            Cost = dto.Cost,
            Status = "draft",
            IsActive = true,
            UpdatedAt = DateTime.UtcNow
        };

        _context.CareerPrograms.Add(program);
        await _context.SaveChangesAsync();

        return await MapToResponse(program);
    }

    
    public async Task<CareerProgramResponseDTO> GetProgramByIdAsync(int programId)
    {
        var program = await _context.CareerPrograms
            .Include(p => p.CareerCounselor)
                .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(p => p.Id == programId && p.IsActive);

        if (program == null)
        {
            throw new Exception("Program not found");
        }

        return await MapToResponse(program);
    }

    public async Task<List<CareerProgramResponseDTO>> GetProgramsByCounselorAsync(int counselorId)
    {
        var programs = await _context.CareerPrograms
            .Include(p => p.CareerCounselor)
                .ThenInclude(c => c.User)
            .Where(p => p.CareerCounselorId == counselorId && p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var result = new List<CareerProgramResponseDTO>();
        foreach (var program in programs)
        {
            result.Add(await MapToResponse(program));
        }

        return result;
    }

    public async Task<List<CareerProgramResponseDTO>> GetAllProgramsAsync(string? status = null, string? type = null)
    {
        var query = _context.CareerPrograms
            .Include(p => p.CareerCounselor)
                .ThenInclude(c => c.User)

            .Where(p => p.IsActive && p.Status != "cancelled")
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status == status);

        if (!string.IsNullOrEmpty(type))
            query = query.Where(p => p.Type == type);

        var programs = await query
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var result = new List<CareerProgramResponseDTO>();
        foreach (var program in programs)
        {
            result.Add(await MapToResponse(program));
        }

        return result;
    }

    public async Task<List<CareerProgramResponseDTO>> GetAvailableProgramsAsync()
    {
        var now = DateTime.UtcNow;

        var programs = await _context.CareerPrograms
            .Include(p => p.CareerCounselor)
                .ThenInclude(c => c.User)
            .Where(p => p.IsActive
                && p.Status == "published"
                && p.RegistrationDeadline >= now
                && p.CurrentParticipants < p.MaxParticipants)
            .OrderBy(p => p.StartDate)
            .ToListAsync();

        var result = new List<CareerProgramResponseDTO>();
        foreach (var program in programs)
        {
            result.Add(await MapToResponse(program));
        }

        return result;
    }

    public async Task<CareerProgramResponseDTO> UpdateProgramAsync(int programId, UpdateCareerProgramDTO dto)
    {
        var program = await _context.CareerPrograms
            .FirstOrDefaultAsync(p => p.Id == programId && p.IsActive);

        if (program == null)
        {
            throw new Exception("Program not found");
        }

        if (!string.IsNullOrEmpty(dto.Title))
            program.Title = dto.Title;

        if (!string.IsNullOrEmpty(dto.Description))
            program.Description = dto.Description;

        if (!string.IsNullOrEmpty(dto.ShortDescription))
            program.ShortDescription = dto.ShortDescription;

        if (dto.IsExternal.HasValue)
            program.IsExternal = dto.IsExternal.Value;

        if (!string.IsNullOrEmpty(dto.ExternalOrganization))
            program.ExternalOrganization = dto.ExternalOrganization;

        if (!string.IsNullOrEmpty(dto.Type))
            program.Type = dto.Type;

        if (!string.IsNullOrEmpty(dto.Level))
            program.Level = dto.Level;

        if (dto.StartDate.HasValue)
            program.StartDate = dto.StartDate.Value;

        if (dto.EndDate.HasValue)
            program.EndDate = dto.EndDate.Value;

        if (dto.RegistrationDeadline.HasValue)
            program.RegistrationDeadline = dto.RegistrationDeadline.Value;

        if (dto.IsOnline.HasValue)
            program.IsOnline = dto.IsOnline.Value;

        if (!string.IsNullOrEmpty(dto.Location))
            program.Location = dto.Location;

        if (!string.IsNullOrEmpty(dto.Venue))
            program.Venue = dto.Venue;

        if (!string.IsNullOrEmpty(dto.MeetingLink))
            program.MeetingLink = dto.MeetingLink;

        if (dto.MaxParticipants.HasValue)
            program.MaxParticipants = dto.MaxParticipants.Value;

        if (dto.IsFree.HasValue)
            program.IsFree = dto.IsFree.Value;

        if (dto.Cost.HasValue)
            program.Cost = dto.Cost;

        if (!string.IsNullOrEmpty(dto.Status))
            program.Status = dto.Status;

        if (dto.IsFeatured.HasValue)
            program.IsFeatured = dto.IsFeatured.Value;

        if (!string.IsNullOrEmpty(dto.ImageUrl))
            program.ImageUrl = dto.ImageUrl;

        program.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await MapToResponse(program);
    }

    public async Task<bool> PublishProgramAsync(int programId)
    {
        var program = await _context.CareerPrograms
            .FirstOrDefaultAsync(p => p.Id == programId && p.IsActive);

        if (program == null)
        {
            throw new Exception("Program not found");
        }

        if (program.Status == "published")
        {
            throw new Exception("Program is already published");
        }

        program.Status = "published";
        program.PublishedAt = DateTime.UtcNow;
        program.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    // ─── DELETE PROGRAM ───
    public async Task<bool> DeleteProgramAsync(int programId)
    {
        var program = await _context.CareerPrograms
            .FirstOrDefaultAsync(p => p.Id == programId && p.IsActive);

        if (program == null)
        {
            throw new Exception("Program not found");
        }

        program.IsActive = false;
        program.Status = "cancelled";
        program.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<ProgramApplicationResponseDTO> ApplyToProgramAsync(
        int programId,
        int studentId,
        string? motivationLetter = null,
        string? currentProgram = null,
        int? currentYearOfStudy = null)
    {
        var program = await _context.CareerPrograms
            .FirstOrDefaultAsync(p => p.Id == programId && p.IsActive);

        if (program == null)
        {
            throw new Exception("Program not found");
        }

        if (program.Status != "published")
        {
            throw new Exception("Program is not open for applications");
        }

        if (program.CurrentParticipants >= program.MaxParticipants)
        {
            throw new Exception("Program is full");
        }

        if (DateTime.UtcNow > program.RegistrationDeadline)
        {
            throw new Exception("Registration deadline has passed");
        }

        var existingApplication = await _context.ProgramApplications
            .FirstOrDefaultAsync(a => a.ProgramId == programId && a.StudentId == studentId);

        if (existingApplication != null)
        {
            throw new Exception("You have already applied to this program");
        }

        var application = new ProgramApplication
        {
            ProgramId = programId,
            StudentId = studentId,
            Status = "pending",
            MotivationLetter = motivationLetter,
            UpdatedAt = DateTime.UtcNow
        };

        _context.ProgramApplications.Add(application);
        await _context.SaveChangesAsync();

        program.CurrentParticipants++;
        if (program.CurrentParticipants >= program.MaxParticipants)
        {
            program.Status = "full";
        }
        program.ApplicationCount++;
        await _context.SaveChangesAsync();

        return await MapApplicationToResponse(application);
    }
    public async Task<List<ProgramApplicationResponseDTO>> GetProgramApplicationsAsync(int programId)
    {
        var applications = await _context.ProgramApplications
            .Include(a => a.Student)
                .ThenInclude(s => s.User)
            .Where(a => a.ProgramId == programId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        var result = new List<ProgramApplicationResponseDTO>();
        foreach (var app in applications)
        {
            result.Add(await MapApplicationToResponse(app));
        }

        return result;
    }

    public async Task<ProgramApplicationResponseDTO> UpdateApplicationStatusAsync(int applicationId, string status)
    {
        var application = await _context.ProgramApplications
            .Include(a => a.Program)
            .FirstOrDefaultAsync(a => a.Id == applicationId);

        if (application == null)
        {
            throw new Exception("Application not found");
        }

        var validStatuses = new[] { "pending", "approved", "rejected", "waitlisted" };
        if (!validStatuses.Contains(status))
        {
            throw new Exception("Invalid status. Valid statuses: pending, approved, rejected, waitlisted");
        }

        application.Status = status;
        application.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await MapApplicationToResponse(application);
    }

    private async Task<CareerProgramResponseDTO> MapToResponse(CareerProgram program)
    {
        return new CareerProgramResponseDTO
        {
            Id = program.Id,
            CareerCounselorId = program.CareerCounselorId,
            Title = program.Title,
            Description = program.Description,
            ShortDescription = program.ShortDescription,
            IsExternal = program.IsExternal,
            ExternalOrganization = program.ExternalOrganization,
            Type = program.Type,
            Level = program.Level,
            StartDate = program.StartDate,
            EndDate = program.EndDate,
            RegistrationDeadline = program.RegistrationDeadline,
            IsOnline = program.IsOnline,
            Location = program.Location,
            Venue = program.Venue,
            MeetingLink = program.MeetingLink,
            MaxParticipants = program.MaxParticipants,
            CurrentParticipants = program.CurrentParticipants,
            IsFree = program.IsFree,
            Cost = program.Cost,
            Status = program.Status,
            PublishedAt = program.PublishedAt,
            IsActive = program.IsActive,
            IsFeatured = program.IsFeatured,
            ImageUrl = program.ImageUrl,
            ApplicationCount = program.ApplicationCount,
            CreatedAt = program.CreatedAt,
            UpdatedAt = program.UpdatedAt,
            IsFull = program.IsFull,
            IsRegistrationOpen = program.IsRegistrationOpen,
            AvailableSpots = program.AvailableSpots,
            CounselorName = program.CareerCounselor?.User?.FullName ?? "Unknown",
            CounselorDepartment = program.CareerCounselor?.Department ?? "Unknown"
        };
    }

    private async Task<ProgramApplicationResponseDTO> MapApplicationToResponse(ProgramApplication application)
    {
        return new ProgramApplicationResponseDTO
        {
            Id = application.Id,
            ProgramId = application.ProgramId,
            ProgramTitle = application.Program?.Title ?? "Unknown",
            StudentId = application.StudentId,
            StudentName = application.Student?.User?.FullName ?? "Unknown",
            StudentEmail = application.Student?.User?.Email ?? "Unknown",
            StudentProgram = application.Student?.Program ?? "Unknown",
            StudentYearOfStudy = application.Student?.YearOfStudy,
            MotivationLetter = application.MotivationLetter,
            Status = application.Status,
            CreatedAt = application.CreatedAt,
            UpdatedAt = application.UpdatedAt
        };
    }
}