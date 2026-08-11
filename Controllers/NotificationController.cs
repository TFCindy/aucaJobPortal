using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AucaJobPortalDotNet.Interfaces;

namespace AucaJobPortalDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(claim)) throw new Exception("User ID not found in token");
        return int.Parse(claim);
    }

    [HttpGet("my-notifications")]
    public async Task<IActionResult> GetMyNotifications()
    {
        var notifications = await _notificationService.GetMyNotificationsAsync(GetUserId());
        return Ok(new
        {
            success = true,
            data = notifications,
            message = notifications.Count == 0 ? "No notifications yet." : $"{notifications.Count} notification(s)."
        });
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var count = await _notificationService.GetUnreadCountAsync(GetUserId());
        return Ok(new { success = true, data = new { unreadCount = count } });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNotificationById(int id)
    {
        var notification = await _notificationService.GetNotificationByIdAsync(GetUserId(), id);
        if (notification == null)
        {
            return NotFound(new { success = false, message = "Notification not found" });
        }
        return Ok(new { success = true, data = notification });
    }

    [HttpPatch("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var success = await _notificationService.MarkOneAsReadAsync(GetUserId(), id);
        if (!success)
        {
            return NotFound(new { success = false, message = "Notification not found" });
        }
        return Ok(new { success = true, message = "Notification marked as read" });
    }

    [HttpPatch("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var count = await _notificationService.MarkAllAsReadAsync(GetUserId());
        return Ok(new { success = true, message = $"{count} notification(s) marked as read" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotification(int id)
    {
        var success = await _notificationService.DeleteNotificationAsync(GetUserId(), id);
        if (!success)
        {
            return NotFound(new { success = false, message = "Notification not found" });
        }
        return Ok(new { success = true, message = "Notification deleted" });
    }
}