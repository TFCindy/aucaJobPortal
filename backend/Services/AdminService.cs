using Microsoft.EntityFrameworkCore;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;
using AucaJobPortalDotNet.Models;

namespace AucaJobPortalDotNet.Services;

public class AdminService : IAdminService
{
    private readonly AucaDbContext _context;

    public AdminService(AucaDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatsDTO> GetDashboardStatsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var totalJobs = await _context.Jobs.CountAsync();
        var pendingJobs = await _context.Jobs.CountAsync(j => j.Status == "pending");
        var totalCounselors = await _context.CareerCounselors.CountAsync(c => c.IsActive);

        var recentUsers = await _context.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(5)
            .Select(u => new RecentUserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                IsActive = u.IsActive,
                Role = u.Role.Name
            })
            .ToListAsync();

        var recentJobs = await _context.Jobs
            .Include(j => j.Employer)
            .OrderByDescending(j => j.PostedAt)
            .Take(5)
            .Select(j => new RecentJobDTO
            {
                Id = j.Id,
                Title = j.Title,
                Status = j.Status,
                CompanyName = j.Employer.CompanyName
            })
            .ToListAsync();

        return new DashboardStatsDTO
        {
            TotalUsers = totalUsers,
            TotalJobs = totalJobs,
            PendingJobs = pendingJobs,
            TotalCounselors = totalCounselors,
            RecentUsers = recentUsers,
            RecentJobs = recentJobs
        };
    }
}