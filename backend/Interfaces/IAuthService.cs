using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Interfaces;
public interface IAuthService
{
    Task<AuthResponseDTO> LoginAsync(LoginRequestDTO dto);
    Task<AuthResponseDTO> RegisterStudentAsync(RegisterStudentDTO dto);
    Task<AuthResponseDTO> RegisterEmployerAsync(RegisterEmployerDTO dto);
    Task<AuthResponseDTO> VerifyEmailAsync(string token);
    Task<PasswordResponseDTO> ChangePasswordAsync( int UserId,ChangePasswordDTO dto);
    Task<PasswordResponseDTO> ForgotPasswordAsync(ForgotPasswordDTO dto);
    Task<PasswordResponseDTO> ResetPasswordAsync(ResetPasswordDTO dto);


}
