using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Services.Interfaces;

public interface IApplicationService
{
    Task<ApplicationResponseDTO> ApplyAsync(int studentId, CreateApplicationDTO dto);
    Task<List<ApplicationResponseDTO>> GetMyApplicationsAsync(int studentId);
    Task<ApplicationResponseDTO> GetApplicationByIdAsync(int applicationId);
    Task<bool> WithdrawApplicationAsync(int applicationId, int studentId);
    Task<List<ApplicationResponseDTO>> GetApplicationsForJobAsync(int jobId, int employerId);
    Task<ApplicationResponseDTO> UpdateApplicationStatusAsync(
        int applicationId, 
        int employerId, 
        UpdateApplicationStatusDTO dto);
    Task<IEnumerable<ApplicationResponseDTO>> GetEmployerApplicationsAsync(int employerId);
}