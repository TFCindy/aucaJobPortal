using AucaJobPortalDotNet.Interfaces;
using AucaJobPortalDotNet.Models;

namespace AucaJobPortalDotNet.Services;

public class NotificationService : INotificationService
{
    private readonly AucaDbContext _context;

    public NotificationService(AucaDbContext context)
    {
        _context = context;
    }

    public async Task CreateNotificationAsync(int userId, string title, string message, string type, string? entityType = null, int? entityId = null)
{
    var notification = new Notification
    {
        UserId = userId,
        Title = title,
        Message = message,
        Type = type,
        EntityType = entityType,
        EntityId = entityId,
        IsRead = false
    };

    _context.Notifications.Add(notification);
    await _context.SaveChangesAsync();
}
}