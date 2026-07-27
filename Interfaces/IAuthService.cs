using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Interfaces;
public interface IAuthService
{
    Task<AuthResponseDTO> LoginAsync(LoginRequestDTO dto);
    Task<AuthResponseDTO> RegisterStudentAsync(RegisterStudentDTO dto);
    Task<AuthResponseDTO> RegisterEmployerAsync(RegisterEmployerDTO dto);

}
