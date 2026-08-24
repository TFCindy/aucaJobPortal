using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Interfaces;

public interface IRoleService
{
    Task<List<RoleDTO>> GetRolesAsync();
    Task<RoleDTO> GetRoleByIdAsync(int roleId);
    Task<List<PermissionDTO>> GetPermissionsAsync();
    Task<bool> UpdateRolePermissionsAsync(int roleId, List<string> permissions);
}