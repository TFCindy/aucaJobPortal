// Services/UserService.cs
using Microsoft.EntityFrameworkCore;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;
using AucaJobPortalDotNet.Models;

namespace AucaJobPortalDotNet.Services;

public class UserService : IUserService
{
    private readonly AucaDbContext _context;

    public UserService(AucaDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDTO>> GetUsersAsync()
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new UserDTO
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Phone = u.Phone,
                IsActive = u.IsActive,
                Role = u.Role.Name,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

        return users;
    }

    public async Task<bool> DeactivateUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ActivateUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        user.IsActive = true;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}