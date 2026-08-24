namespace AucaJobPortalDotNet.Interfaces;

public interface IUserService
{
    Task<List<UserDTO>> GetUsersAsync();
    Task<bool> DeactivateUserAsync(int userId);
    Task<bool> ActivateUserAsync(int userId);
}