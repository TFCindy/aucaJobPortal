namespace AucaJobPortalDotNet.Interfaces;
public interface IPermissionService
{
    Task<bool> HasPermissionAsync(int roleId, string permissionName);
}