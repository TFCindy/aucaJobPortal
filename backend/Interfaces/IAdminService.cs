using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Interfaces;

public interface IAdminService
{
    Task<DashboardStatsDTO> GetDashboardStatsAsync();
}