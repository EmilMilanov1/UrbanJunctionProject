using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class NotificationsController : Controller
{
    private readonly INotificationService _notificationService;
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> Dropdown()
    {
        var notifications = await _notificationService.GetForUserAsync(UserId);
        var unreadCount = await _notificationService.GetUnreadCountAsync(UserId);
        ViewBag.UnreadCount = unreadCount;
        return PartialView("_NotificationDropdown", notifications);
    }

    [HttpGet]
    public async Task<IActionResult> UnreadCount()
    {
        var count = await _notificationService.GetUnreadCountAsync(UserId);
        return Json(new { count });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkRead(int id)
    {
        await _notificationService.MarkReadAsync(id, UserId);
        var count = await _notificationService.GetUnreadCountAsync(UserId);
        return Json(new { success = true, unreadCount = count });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAllRead()
    {
        await _notificationService.MarkAllReadAsync(UserId);
        return Json(new { success = true, unreadCount = 0 });
    }
}