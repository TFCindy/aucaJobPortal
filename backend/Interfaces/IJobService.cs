using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Interfaces;

public interface IJobService
{
    Task<List<JobResponseDTO>> GetAllJobsAsync();
    Task<List<JobResponseDTO>> GetActiveJobsAsync();
    Task<List<JobResponseDTO>> SearchJobsAsync(List<int>? jobTypeIds, string? location);
    Task<JobResponseDTO> GetJobByIdAsync(int jobId);
    Task<JobResponseDTO> CreateJobAsync(CreateJobDTO dto, int employerId);
    Task<JobResponseDTO> UpdateJobAsync(int jobId, UpdateJobDTO dto);
    Task<bool> WithdrawJobAsync(int jobId);
    Task<JobResponseDTO> ApproveJobAsync(int jobId, int approvedByUserId);
    Task<JobResponseDTO> RejectJobAsync(int jobId, int rejectedByUserId);
    Task<List<JobResponseDTO>> GetJobsByEmployerUserIdAsync(int userId);

}