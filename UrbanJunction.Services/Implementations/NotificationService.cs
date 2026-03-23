using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data;
using UrbanJunction.Data.Models;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;

    public NotificationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(string userId, string? actorId, NotificationType type,
        int? postId = null, int? commentId = null, int? contactMessageId = null)
    {
        if (actorId != null && actorId == userId) return;

        bool duplicate = await _context.Notifications.AnyAsync(n =>
            n.UserId == userId &&
            n.ActorId == actorId &&
            n.Type == type &&
            n.PostId == postId &&
            n.CommentId == commentId &&
            n.ContactMessageId == contactMessageId &&
            !n.IsRead);

        if (duplicate) return;

        _context.Notifications.Add(new Notification
        {
            UserId = userId,
            ActorId = actorId,
            Type = type,
            PostId = postId,
            CommentId = commentId,
            ContactMessageId = contactMessageId,
            CreatedOn = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Notification>> GetForUserAsync(string userId, int take = 20)
    {
        return await _context.Notifications
            .Include(n => n.Actor)
            .Include(n => n.Post)
            .Include(n => n.Comment)
            .Include(n => n.ContactMessage)
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedOn)
            .Take(take)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        return await _context.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead);
    }

    public async Task MarkReadAsync(int notificationId, string userId)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

        if (notification == null) return;

        notification.IsRead = true;
        await _context.SaveChangesAsync();
    }

    public async Task MarkAllReadAsync(string userId)
    {
        var unread = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var n in unread)
            n.IsRead = true;

        await _context.SaveChangesAsync();
    }
}