using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Services.Interfaces;

public interface IProfileService
{
    Task<StudentProfileDTO> GetStudentProfileAsync(int userId);
    Task<StudentProfileDTO> UpdateStudentProfileAsync(int userId, UpdateStudentProfileDTO dto);
    Task<EmployerProfileDTO> GetEmployerProfileAsync(int userId);
    Task<EmployerProfileDTO> UpdateEmployerProfileAsync(int userId, UpdateEmployerProfileDTO dto);
    Task<AlumniProfileDTO> GetAlumniProfileAsync(int userId);
    Task<AlumniProfileDTO> UpdateAlumniProfileAsync(int userId, UpdateAlumniProfileDTO dto);
    Task<CareerCounselorProfileDTO> GetCareerCounselorProfileAsync(int userId);
    Task<CareerCounselorProfileDTO> UpdateCareerCounselorProfileAsync(int userId, UpdateCareerCounselorProfileDTO dto);
}