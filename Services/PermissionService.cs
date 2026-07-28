using AucaJobPortalDotNet.Interfaces;
using AucaJobPortalDotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace AucaJobPortalDotNet.Services;
public class PermissionService : IPermissionService
{
   private readonly AucaDbContext _context;

   public PermissionService(
    AucaDbContext context
   )

   {_context=context;}

   public async Task<bool> HasPermissionAsync(int roleId, string permissionName)
    {
        return await _context.RolePermissions
            .AnyAsync(rp => rp.RoleId == roleId && rp.Permission.Name == permissionName);
    }
}