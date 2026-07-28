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

    // REGISTER STUDENT 
    public async Task<string> RegisterStudentAsync(RegisterStudentDTO dto)
    {
        // 1. Check if email exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower());

        if (existingUser != null)
        {
            return "Email already registered";
        }

        // 2. Hash password
        var hashedPassword = _passwordHasher.HashPassword(dto.Password);

        // 3. Generate verification token
        var verificationToken = Guid.NewGuid().ToString("N");
        var verificationExpires = DateTime.UtcNow.AddHours(24);

        // 4. Get Student role
        var studentRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == "student");

        if (studentRole == null)
        {
            return "Student role not found";
        }

        // 5. Create User
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
            // ✅ CreatedAt auto-sets
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // 6. Create Student
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

        // 7. Send verification email
        await _emailService.SendVerificationEmailAsync(user.Email, user.FullName, verificationToken);

        // 8. Return success message
        return "Registration successful. Please check your email to verify your account.";
    }

    // ─── REGISTER EMPLOYER ───
    public async Task<string> RegisterEmployerAsync(RegisterEmployerDTO dto)
    {
        // 1. Check if email exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower());

        if (existingUser != null)
        {
            return "Email already registered";
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
            return "Employer role not found";
        }

        // 5. Create User
        var user = new User
        {
            Email = dto.Email.ToLower(),
            PasswordHash = hashedPassword,
            FullName = dto.FullName,
            Phone = dto.Phone,
            RoleId = employerRole.Id,
            EmailVerified = false,
            EmailVerificationToken = verificationToken,
            EmailVerificationExpires = verificationExpires,
            IsActive = true,
            UpdatedAt = DateTime.UtcNow
            // ✅ CreatedAt auto-sets
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // 6. Create Employer
        var employer = new Employer
        {
            UserId = user.Id,
            CompanyName = dto.CompanyName,
            CompanyDescription = dto.CompanyDescription,
            CompanyWebsite = dto.CompanyWebsite,
            Industry = dto.Industry,
            CompanySize = dto.CompanySize,
            Headquarters = dto.HeadQuarters
            // ✅ CreatedAt auto-sets
        };

        _context.Employers.Add(employer);
        await _context.SaveChangesAsync();

        // 7. Send verification email
        await _emailService.SendVerificationEmailAsync(user.Email, user.FullName, verificationToken);

        // 8. Return success message
        return "Registration successful. Please check your email to verify your account.";
    }

    // ─── VERIFY EMAIL ───
    public async Task<string> VerifyEmailAsync(string token)
    {
        // 1. Find user by token
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.EmailVerificationToken == token);

        if (user == null)
        {
            return "Invalid verification link";
        }

        // 2. Check if already verified
        if (user.EmailVerified)
        {
            return "Email already verified";
        }

        // 3. Check if token expired (24 hours)
        if (user.EmailVerificationExpires < DateTime.UtcNow)
        {
            return "Verification link has expired. Please request a new one.";
        }

        // 4. Mark as verified
        user.EmailVerified = true;
        user.EmailVerificationToken = null;
        user.EmailVerificationExpires = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // 5. Send welcome email
        await _emailService.SendWelcomeEmailAsync(user.Email, user.FullName);

        // 6. Return success
        return "Email verified successfully! You can now log in.";
    }

    // ─── LOGIN ───
    public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO dto)
    {
        // 1. Find user by email
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower());

        if (user == null)
        {
            return new AuthResponseDTO
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        // 2. Check if email is verified
        if (!user.EmailVerified)
        {
            return new AuthResponseDTO
            {
                Success = false,
                Message = "Please verify your email before logging in. Check your inbox for the verification link."
            };
        }

        // 3. Check password
        if (!_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
        {
            return new AuthResponseDTO
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        // 4. Check if account is active
        if (!user.IsActive)
        {
            return new AuthResponseDTO
            {
                Success = false,
                Message = "Your account has been deactivated. Please contact support."
            };
        }

        // 5. Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        // 6. Generate JWT token
        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email,
            user.RoleId,
            user.Role?.Name ?? "user"
        );

        // 7. Return response
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