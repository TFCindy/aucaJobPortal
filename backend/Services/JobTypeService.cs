using Microsoft.EntityFrameworkCore;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;
using AucaJobPortalDotNet.Models;

namespace AucaJobPortalDotNet.Services;

public class JobTypeService : IJobTypeService
{
    private readonly AucaDbContext _context;

    public JobTypeService(AucaDbContext context)
    {
        _context = context;
    }

    public async Task<List<JobTypeDTO>> GetAllJobTypesAsync()
    {
        var jobTypes = await _context.JobTypes
            .OrderBy(jt => jt.Name)
            .Select(jt => new JobTypeDTO
            {
                Id = jt.Id,
                Name = jt.Name,
                Description = jt.Description,
                GroupName = jt.GroupName
            })
            .ToListAsync();

        return jobTypes;
    }
}