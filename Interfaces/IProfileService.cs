using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Services.Interfaces;

public interface IProfileService
{
    Task<StudentProfileDTO> GetStudentProfileAsync(int userId);
    Task<StudentProfileDTO> UpdateStudentProfileAsync(int userId, UpdateStudentProfileDTO dto);
    Task<EmployerProfileDTO> GetEmployerProfileAsync(int userId);
    Task<EmployerProfileDTO> UpdateEmployerProfileAsync(int userId, UpdateEmployerProfileDTO dto);
}