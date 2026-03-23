using UrbanJunction.Data.Models;

public interface INotificationService
{
    Task CreateAsync(string userId, string? actorId, NotificationType type,
        int? postId = null, int? commentId = null, int? contactMessageId = null);

    Task<IEnumerable<Notification>> GetForUserAsync(string userId, int take = 20);
    Task<int> GetUnreadCountAsync(string userId);
    Task MarkReadAsync(int notificationId, string userId);
    Task MarkAllReadAsync(string userId);
}