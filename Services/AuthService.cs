using Microsoft.EntityFrameworkCore;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Helpers;
using AucaJobPortalDotNet.Models;
using AucaJobPortalDotNet.Interfaces;

namespace AucaJobPortalDotNet.Services;

public class AuthService : IAuthService
{
    private readonly AucaDbContext _context;
    private readonly PasswordHasher _passwordHasher;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly IEmailService _emailService;

    public AuthService(
        AucaDbContext context,
        PasswordHasher passwordHasher,
        JwtTokenGenerator jwtTokenGenerator,
        IEmailService emailService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _emailService = emailService;
    }

    // ─── REGISTER STUDENT ───
    public async Task<AuthResponseDTO> RegisterStudentAsync(RegisterStudentDTO dto)
    {
        // 1. Check if email exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower());

        if (existingUser != null)
        {
            return new AuthResponseDTO { Success = false, Message = "Email already registered" };
        }

        // 2. Check if StudentId already exists
        var existingStudentId = await _context.Students
            .FirstOrDefaultAsync(s => s.StudentId == dto.StudentId);

        if (existingStudentId != null)
        {
            return new AuthResponseDTO { Success = false, Message = "This Student ID is already registered" };
        }

        // 3. Hash password
        var hashedPassword = _passwordHasher.HashPassword(dto.Password);

        // 4. Generate verification token
        var verificationToken = Guid.NewGuid().ToString("N");
        var verificationExpires = DateTime.UtcNow.AddHours(24);

        // 5. Get Student role
        var studentRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == "student");

        if (studentRole == null)
        {
            return new AuthResponseDTO { Success = false, Message = "Student role not found" };
        }

        // 6. Create User + Student together, inside a transaction
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = new User
            {
                Email = dto.Email.ToLower(),
                PasswordHash = hashedPassword,
                FullName = dto.FullName,
                Phone = dto.Phone,
                RoleId = studentRole.Id,
                EmailVerified = false,
                EmailVerificationToken = verificationToken,
                EmailVerificationExpires = verificationExpires,
                IsActive = true,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var student = new Student
            {
                UserId = user.Id,
                StudentId = dto.StudentId,
                Program = dto.Program,
                Faculty = dto.Faculty,
                Department = dto.Department,
                YearOfStudy = dto.YearOfStudy,
                GraduationYear = dto.GraduationYear
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            // 7. Send verification email (after commit, so we don't email
            //    someone whose registration ultimately failed)
            await _emailService.SendVerificationEmailAsync(user.Email, user.FullName, verificationToken);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw; // let ExceptionHandlingMiddleware catch anything unexpected
        }

        return new AuthResponseDTO
        {
            Success = true,
            Message = "Registration successful. Please check your email to verify your account."
        };
    }

    // ─── REGISTER EMPLOYER ───
    public async Task<AuthResponseDTO> RegisterEmployerAsync(RegisterEmployerDTO dto)
    {
        // 1. Check if email exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower());

        if (existingUser != null)
        {
            return new AuthResponseDTO { Success = false, Message = "Email already registered" };
        }

        var existingCompany = await _context.Employers
            .FirstOrDefaultAsync(e => e.CompanyName.ToLower() == dto.CompanyName.ToLower());

        if (existingCompany != null)
        {
            return new AuthResponseDTO { Success = false, Message = "A company with this name is already registered" };
        }
        
        // 2. Hash password
        var hashedPassword = _passwordHasher.HashPassword(dto.Password);

        // 3. Generate verification token
        var verificationToken = Guid.NewGuid().ToString("N");
        var verificationExpires = DateTime.UtcNow.AddHours(24);

        // 4. Get Employer role
        var employerRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == "employer");

        if (employerRole == null)
        {
            return new AuthResponseDTO { Success = false, Message = "Employer role not found" };
        }

        // 5. Create User + Employer together, inside a transaction
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = new User
            {
                Email = dto.Email.ToLower(),
                PasswordHash = hashedPassword,
                FullName = dto.CompanyName,
                Phone = dto.Phone,
                RoleId = employerRole.Id,
                EmailVerified = false,
                EmailVerificationToken = verificationToken,
                EmailVerificationExpires = verificationExpires,
                IsActive = true,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var employer = new Employer
            {
                UserId = user.Id,
                CompanyName = dto.CompanyName,
                CompanyDescription = dto.CompanyDescription,
                CompanyWebsite = dto.CompanyWebsite,
                Industry = dto.Industry,
                CompanySize = dto.CompanySize,
                Location = dto.Location,
                Tin=dto.Tin
            };

            _context.Employers.Add(employer);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            // 6. Send verification email (after commit)
            await _emailService.SendVerificationEmailAsync(user.Email, user.FullName, verificationToken);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return new AuthResponseDTO
        {
            Success = true,
            Message = "Registration successful. Please check your email to verify your account."
        };
    }

    // ─── VERIFY EMAIL ───
    public async Task<AuthResponseDTO> VerifyEmailAsync(string token)
{
    var user = await _context.Users
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.EmailVerificationToken == token);

    if (user == null)
    {
        return new AuthResponseDTO { Success = false, Message = "Invalid verification link" };
    }

    if (user.EmailVerified)
    {
        return new AuthResponseDTO { Success = true, Message = "Email already verified" };
    }

    if (user.EmailVerificationExpires < DateTime.UtcNow)
    {
        return new AuthResponseDTO { Success = false, Message = "Verification link has expired. Please request a new one." };
    }

    user.EmailVerified = true;
    user.EmailVerificationToken = null;
    user.EmailVerificationExpires = null;
    user.UpdatedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();

    var userType = user.Role?.Name ?? "student";
    await _emailService.SendWelcomeEmailAsync(user.Email, user.FullName, userType);

    return new AuthResponseDTO { Success = true, Message = "Email verified successfully! You can now log in." };
}

    // ─── LOGIN ───
    public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO dto)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower());

        if (user == null)
        {
            return new AuthResponseDTO { Success = false, Message = "Invalid email or password" };
        }

        if (!user.EmailVerified)
        {
            return new AuthResponseDTO { Success = false, Message = "Please verify your email before logging in. Check your inbox for the verification link." };
        }

        if (!_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
        {
            return new AuthResponseDTO { Success = false, Message = "Invalid email or password" };
        }

        if (!user.IsActive)
        {
            return new AuthResponseDTO { Success = false, Message = "Your account has been deactivated. Please contact support." };
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email,
            user.RoleId,
            user.Role?.Name ?? "user"
        );

        return new AuthResponseDTO
        {
            Success = true,
            Message = "Login successful",
            Token = token,
            User = new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role?.Name ?? "user"
            }
        };
    }
}