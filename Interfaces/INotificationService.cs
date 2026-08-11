using AucaJobPortalDotNet.DTOs;

namespace AucaJobPortalDotNet.Interfaces;

public interface INotificationService
{
    Task CreateNotificationAsync(int userId, string title, string message, string type, string? entityType = null, int? entityId = null);
    Task<List<NotificationResponseDTO>> GetMyNotificationsAsync(int userId);
    Task<NotificationResponseDTO?> GetNotificationByIdAsync(int userId, int notificationId);
    Task<bool> MarkOneAsReadAsync(int userId, int notificationId);
    Task<int> MarkAllAsReadAsync(int userId);
    Task<int> GetUnreadCountAsync(int userId);
    Task<bool> DeleteNotificationAsync(int userId, int notificationId);
}