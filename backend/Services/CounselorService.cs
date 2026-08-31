using Microsoft.EntityFrameworkCore;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;
using AucaJobPortalDotNet.Models;
using AucaJobPortalDotNet.Helpers;

namespace AucaJobPortalDotNet.Services;

public class CounselorService : ICounselorService
{
    private readonly AucaDbContext _context;
    private readonly PasswordHasher _passwordHasher;

    public CounselorService(AucaDbContext context, PasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<CounselorResponseDTO> CreateCounselorAsync(CreateCounselorDTO dto)
    {
        // 1. Get the career_counselor role
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == "career_counselor");

        if (role == null)
        {
            throw new Exception("Career Counselor role not found. Please seed the database first.");
        }
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (existingUser != null)
        {
            throw new Exception("A user with this email already exists.");
        }

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = _passwordHasher.HashPassword(dto.Password),
            FullName = dto.FullName,
            Phone = dto.Phone,
            RoleId = role.Id,
            IsActive = true,
            EmailVerified = true,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var counselor = new CareerCounselor
        {
            UserId = user.Id,
            Department = dto.Department,
            Title = dto.Title,
            Bio = dto.Bio,
            Specialization = dto.Specialization,
            YearsOfExperience = dto.YearsOfExperience,
            OfficeLocation = dto.OfficeLocation,
            IsActive = true,
            UpdatedAt = DateTime.UtcNow
        };

        _context.CareerCounselors.Add(counselor);
        await _context.SaveChangesAsync();
        
        return new CounselorResponseDTO
        {
            Id = counselor.Id,
            UserId = user.Id,
            Department = counselor.Department,
            Title = counselor.Title,
            Bio = counselor.Bio,
            Specialization = counselor.Specialization,
            YearsOfExperience = counselor.YearsOfExperience,
            OfficeLocation = counselor.OfficeLocation,
            IsActive = counselor.IsActive,
            CreatedAt = counselor.CreatedAt,
            Email = user.Email,
            FullName = user.FullName,
            Phone = user.Phone
        };
    }

    public async Task<List<CounselorResponseDTO>> GetAllCounselorsAsync()
    {
        var counselors = await _context.CareerCounselors
            .Include(c => c.User)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CounselorResponseDTO
            {
                Id = c.Id,
                UserId = c.UserId,
                Department = c.Department,
                Title = c.Title,
                Bio = c.Bio,
                Specialization = c.Specialization,
                YearsOfExperience = c.YearsOfExperience,
                OfficeLocation = c.OfficeLocation,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                Email = c.User.Email,
                FullName = c.User.FullName,
                Phone = c.User.Phone
            })
            .ToListAsync();

        return counselors;
    }

    public async Task<CounselorResponseDTO> GetCounselorByIdAsync(int id)
    {
        var counselor = await _context.CareerCounselors
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (counselor == null)
        {
            throw new Exception("Counselor not found");
        }

        return new CounselorResponseDTO
        {
            Id = counselor.Id,
            UserId = counselor.UserId,
            Department = counselor.Department,
            Title = counselor.Title,
            Bio = counselor.Bio,
            Specialization = counselor.Specialization,
            YearsOfExperience = counselor.YearsOfExperience,
            OfficeLocation = counselor.OfficeLocation,
            IsActive = counselor.IsActive,
            CreatedAt = counselor.CreatedAt,
            Email = counselor.User.Email,
            FullName = counselor.User.FullName,
            Phone = counselor.User.Phone
        };
    }

    public async Task<bool> DeactivateCounselorAsync(int id)
    {
        var counselor = await _context.CareerCounselors
            .FirstOrDefaultAsync(c => c.Id == id);

        if (counselor == null) return false;

        counselor.IsActive = false;
        counselor.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ActivateCounselorAsync(int id)
    {
        var counselor = await _context.CareerCounselors
            .FirstOrDefaultAsync(c => c.Id == id);

        if (counselor == null) return false;

        counselor.IsActive = true;
        counselor.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}