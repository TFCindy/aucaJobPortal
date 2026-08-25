using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Interfaces;

public interface ICareerProgramService
{
    // ─── PROGRAM CRUD ───
    Task<CareerProgramResponseDTO> CreateProgramAsync(int counselorId, CreateCareerProgramDTO dto);
    Task<CareerProgramResponseDTO> GetProgramByIdAsync(int programId);
    Task<List<CareerProgramResponseDTO>> GetProgramsByCounselorAsync(int counselorId);
    Task<List<CareerProgramResponseDTO>> GetAllProgramsAsync(string? status = null, string? type = null);
    Task<List<CareerProgramResponseDTO>> GetAvailableProgramsAsync();
    Task<CareerProgramResponseDTO> UpdateProgramAsync(int programId, UpdateCareerProgramDTO dto);
    Task<bool> DeleteProgramAsync(int programId);
    Task<bool> PublishProgramAsync(int programId);

    // ─── APPLICATIONS ───
    Task<ProgramApplicationResponseDTO> ApplyToProgramAsync(
        int programId, 
        int studentId, 
        string? motivationLetter = null,
        string? currentProgram = null,
        int? currentYearOfStudy = null);
    
    Task<List<ProgramApplicationResponseDTO>> GetProgramApplicationsAsync(int programId);
    Task<ProgramApplicationResponseDTO> UpdateApplicationStatusAsync(int applicationId, string status);
}