using Microsoft.EntityFrameworkCore;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Models;
using AucaJobPortalDotNet.Interfaces;

namespace AucaJobPortalDotNet.Services;

public class JobService : IJobService
{
    private readonly AucaDbContext _context;

    public JobService(AucaDbContext context)
    {
        _context = context;
    }

    private IQueryable<Job> JobsWithIncludes()
    {
        return _context.Jobs
            .Include(j => j.Employer)
                .ThenInclude(e => e.User)
            .Include(j => j.JobJobTypes)
                .ThenInclude(jjt => jjt.JobType);
    }

    // ─── COMPLETE VALIDATION METHOD ───
    private async Task ValidateJobTypesAsync(List<int> jobTypeIds)
    {
        if (jobTypeIds == null || !jobTypeIds.Any()) return;

        // ─── 1. VALIDATE IDS EXIST ───
        var validTypeIds = await _context.JobTypes
            .Where(jt => jobTypeIds.Contains(jt.Id))
            .Select(jt => jt.Id)
            .ToListAsync();

        var invalidTypes = jobTypeIds.Except(validTypeIds).ToList();
        if (invalidTypes.Any())
        {
            throw new Exception($"Invalid job type ID(s): {string.Join(", ", invalidTypes)}");
        }

        // ─── 2. VALIDATE GROUPS ───
        var jobTypes = await _context.JobTypes
            .Where(jt => jobTypeIds.Contains(jt.Id))
            .ToListAsync();

        var groups = jobTypes.GroupBy(jt => jt.GroupName);
        
        foreach (var group in groups)
        {
            if (group.Count() > 1)
            {
                throw new Exception($"Cannot select multiple job types from the same group ({group.Key}).");
            }
        }
    }

    public async Task<List<JobResponseDTO>> GetAllJobsAsync()
    {
        var jobs = await JobsWithIncludes()
            .OrderByDescending(j => j.PostedAt)
            .ToListAsync();

        return jobs.Select(MapToResponse).ToList();
    }


    public async Task<JobResponseDTO> GetJobByIdAsync(int jobId)
    {
        var job = await JobsWithIncludes()
            .FirstOrDefaultAsync(j => j.Id == jobId);

        if (job == null)
        {
            throw new Exception("Job not found");
        }

        return MapToResponse(job);
    }

    public async Task<JobResponseDTO> CreateJobAsync(CreateJobDTO dto, int employerId)
    {
        //  COMPLETE VALIDATION (BOTH IDS AND GROUPS)
        await ValidateJobTypesAsync(dto.JobTypeIds);

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var job = new Job
            {
                EmployerId = employerId,
                Title = dto.Title,
                Description = dto.Description,
                Requirements = dto.Requirements,
                Responsibilities = dto.Responsibilities,
                Location = dto.Location,
                DeadlineDate = dto.DeadlineDate,
                Openings = dto.Openings,
                Status = "pending",
                UpdatedAt = DateTime.UtcNow,
                ViewsCount = 0
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            if (dto.JobTypeIds != null && dto.JobTypeIds.Any())
            {
                foreach (var typeId in dto.JobTypeIds)
                {
                    _context.JobJobTypes.Add(new JobJobType { JobId = job.Id, JobTypeId = typeId });
                }
                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return await GetJobByIdAsync(job.Id);
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync();
            var innerMessage = ex.InnerException?.Message ?? ex.Message;
            if (innerMessage.Contains("FK_"))
            {
                throw new Exception("Unable to create job. Please check that all references (employer, job types) are valid.");
            }
            throw new Exception($"Unable to create job: {innerMessage}");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<JobResponseDTO> UpdateJobAsync(int jobId, UpdateJobDTO dto)
    {
        var job = await _context.Jobs
            .Include(j => j.JobJobTypes)
            .FirstOrDefaultAsync(j => j.Id == jobId);

        if (job == null)
        {
            throw new Exception("Job not found");
        }

        if (!string.IsNullOrEmpty(dto.Title))
            job.Title = dto.Title;

        if (!string.IsNullOrEmpty(dto.Description))
            job.Description = dto.Description;

        if (!string.IsNullOrEmpty(dto.Requirements))
            job.Requirements = dto.Requirements;

        if (!string.IsNullOrEmpty(dto.Responsibilities))
            job.Responsibilities = dto.Responsibilities;

        if (!string.IsNullOrEmpty(dto.Location))
            job.Location = dto.Location;

        if (dto.DeadlineDate.HasValue)
            job.DeadlineDate = dto.DeadlineDate.Value;

        if (dto.Openings.HasValue)
            job.Openings = dto.Openings.Value;


        if (dto.JobTypeIds != null)
        {
            //  COMPLETE VALIDATION BEFORE UPDATE (BOTH IDS AND GROUPS)
            await ValidateJobTypesAsync(dto.JobTypeIds);

            _context.JobJobTypes.RemoveRange(job.JobJobTypes);

            foreach (var typeId in dto.JobTypeIds)
            {
                _context.JobJobTypes.Add(new JobJobType { JobId = job.Id, JobTypeId = typeId });
            }
        }

        job.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return await GetJobByIdAsync(job.Id);
    }

    public async Task<bool> WithdrawJobAsync(int jobId)
    {
        var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId);

        if (job == null)
        {
            throw new Exception("Job not found");
        }

        job.Status = "withdrawn";
        job.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<JobResponseDTO> ApproveJobAsync(int jobId, int approvedByUserId)
    {
        var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId);

        if (job == null)
        {
            throw new Exception("Job not found");
        }

        job.Status = "active";
        job.ApprovedBy = approvedByUserId;
        job.ApprovedAt = DateTime.UtcNow;
        job.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return await GetJobByIdAsync(job.Id);
    }

    public async Task<JobResponseDTO> RejectJobAsync(int jobId, int rejectedByUserId)
    {
        var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId);

        if (job == null)
        {
            throw new Exception("Job not found");
        }

        job.Status = "rejected";
        job.ApprovedBy = rejectedByUserId;
        job.ApprovedAt = DateTime.UtcNow;
        job.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return await GetJobByIdAsync(job.Id);
    }

    public async Task<List<JobResponseDTO>> GetJobsByEmployerAsync(int employerId)
    {
        var jobs = await JobsWithIncludes()
            .Where(j => j.EmployerId == employerId)
            .OrderByDescending(j => j.PostedAt)
            .ToListAsync();

        return jobs.Select(MapToResponse).ToList();
    }

    public async Task<List<JobResponseDTO>> GetJobsByEmployerUserIdAsync(int userId)
    {
        var employer = await _context.Employers.FirstOrDefaultAsync(e => e.UserId == userId);

        if (employer == null)
        {
            throw new Exception("Employer profile not found. Please complete your employer profile first.");
        }

        return await GetJobsByEmployerAsync(employer.Id);
    }

    private JobResponseDTO MapToResponse(Job job)
    {
        return new JobResponseDTO
        {
            Id = job.Id,
            EmployerId = job.EmployerId,
            CompanyName = job.Employer?.CompanyName ?? "Unknown Company",
            Title = job.Title,
            Description = job.Description,
            Requirements = job.Requirements,
            Responsibilities = job.Responsibilities,
            Location = job.Location,
            JobTypes = job.JobJobTypes?.Select(jjt => jjt.JobType.Name).ToList() ?? new List<string>(),
            Deadline = job.DeadlineDate,
            Status = job.Status,
            PostedAt = job.PostedAt,
            UpdatedAt = job.UpdatedAt,
            ViewsCount = job.ViewsCount,
            Openings = job.Openings,
            ApprovedBy = job.ApprovedBy,
            ApprovedAt = job.ApprovedAt
        };
    }

    public async Task<List<JobResponseDTO>> GetActiveJobsAsync()
    {
        var jobs = await JobsWithIncludes()
            .Where(j => j.Status == "active")
            .OrderByDescending(j => j.PostedAt)
            .ToListAsync();

        return jobs.Select(MapToResponse).ToList();
    }

    // PROTECTED: Search active jobs with filters
    public async Task<List<JobResponseDTO>> SearchJobsAsync(List<int>? jobTypeIds, string? location)
{
    var query = JobsWithIncludes().Where(j => j.Status == "active");

    // MULTIPLE JOB TYPES 
    if (jobTypeIds != null && jobTypeIds.Any())
    {
        query = query.Where(j => 
            j.JobJobTypes.Any(jjt => jobTypeIds.Contains(jjt.JobTypeId))
        );
    }

    if (!string.IsNullOrEmpty(location))
    {
        query = query.Where(j => j.Location != null && j.Location.Contains(location));
    }

    var jobs = await query.OrderByDescending(j => j.PostedAt).ToListAsync();
    return jobs.Select(MapToResponse).ToList();
}
}