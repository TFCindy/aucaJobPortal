using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Interfaces;

public interface ICounselorService
{
    Task<CounselorResponseDTO> CreateCounselorAsync(CreateCounselorDTO dto);
    Task<List<CounselorResponseDTO>> GetAllCounselorsAsync();
    Task<CounselorResponseDTO> GetCounselorByIdAsync(int id);
    Task<bool> DeactivateCounselorAsync(int id);
    Task<bool> ActivateCounselorAsync(int id);
}