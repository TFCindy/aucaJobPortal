using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Interfaces;
public interface IAuthService
{
    Task<AuthResponseDTO> LoginAsync(LoginRequestDTO dto);
    Task<string> RegisterStudentAsync(RegisterStudentDTO dto);
    Task<string> RegisterEmployerAsync(RegisterEmployerDTO dto);
    Task<string> VerifyEmailAsync(string token);

}
