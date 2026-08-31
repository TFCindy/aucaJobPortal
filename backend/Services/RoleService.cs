using Microsoft.EntityFrameworkCore;
using AucaJobPortalDotNet.DTOs;
using AucaJobPortalDotNet.Interfaces;
using AucaJobPortalDotNet.Models;

namespace AucaJobPortalDotNet.Services;

public class RoleService : IRoleService
{
    private readonly AucaDbContext _context;

    public RoleService(AucaDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoleDTO>> GetRolesAsync()
    {
        var roles = await _context.Roles
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .Select(r => new RoleDTO
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Permissions = r.RolePermissions
                    .Select(rp => rp.Permission.Name)
                    .ToList()
            })
            .ToListAsync();

        return roles;
    }

    public async Task<RoleDTO> GetRoleByIdAsync(int roleId)
    {
        var role = await _context.Roles
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == roleId);

        if (role == null)
        {
            throw new Exception("Role not found");
        }

        return new RoleDTO
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            Permissions = role.RolePermissions
                .Select(rp => rp.Permission.Name)
                .ToList()
        };
    }

    public async Task<List<PermissionDTO>> GetPermissionsAsync()
    {
        var permissions = await _context.Permissions
            .Select(p => new PermissionDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            })
            .ToListAsync();

        return permissions;
    }

    public async Task<bool> UpdateRolePermissionsAsync(int roleId, List<string> permissions)
{
    // 1. Handle null permissions
    if (permissions == null)
    {
        permissions = new List<string>();
    }

    // 2. Get the role with permissions INCLUDING the Permission navigation property
    var role = await _context.Roles
        .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)  // ← ADD THIS
        .FirstOrDefaultAsync(r => r.Id == roleId);

    if (role == null) return false;

    // 3. GUARDRAIL: Prevent locking out all admins
    var isRemovingManageRoles = false;

    // Check if the role currently has manage_roles
    if (role.RolePermissions != null && role.RolePermissions.Any())
    {
        isRemovingManageRoles = role.RolePermissions
            .Any(rp => rp.Permission != null && rp.Permission.Name == "manage_roles") &&
            !permissions.Contains("manage_roles");
    }

    if (isRemovingManageRoles)
    {
        // Check if any OTHER role still has manage_roles
        var otherRolesWithManageRoles = await _context.Roles
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .Where(r => r.Id != roleId)
            .SelectMany(r => r.RolePermissions)
            .AnyAsync(rp => rp.Permission != null && rp.Permission.Name == "manage_roles");

        if (!otherRolesWithManageRoles)
        {
            throw new Exception(
                "Cannot remove 'manage_roles' from the last role that has it. " +
                "This would lock everyone out of role management."
            );
        }
    }
    if (role.RolePermissions != null && role.RolePermissions.Any())
    {
        _context.RolePermissions.RemoveRange(role.RolePermissions);
    }

    foreach (var permissionName in permissions)
    {
        var permission = await _context.Permissions
            .FirstOrDefaultAsync(p => p.Name == permissionName);

        if (permission != null)
        {
            _context.RolePermissions.Add(new RolePermission
            {
                RoleId = role.Id,
                PermissionId = permission.Id
            });
        }
    }

    await _context.SaveChangesAsync();
    return true;
}
}