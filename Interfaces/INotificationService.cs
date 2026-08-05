namespace AucaJobPortalDotNet.Interfaces;
public interface INotificationService
{
    Task CreateNotificationAsync(int userId, string title, string message, string type, string? entityType = null, int? entityId = null);
}
