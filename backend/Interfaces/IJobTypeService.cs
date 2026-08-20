using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Interfaces;

public interface IJobTypeService
{
    Task<List<JobTypeDTO>> GetAllJobTypesAsync();
}