using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Interfaces;

namespace UrbanJunction.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public CommentService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<Comment>> GetByPostAsync(int postId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.CreatedOn)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(int postId, string content, string userId, int? parentCommentId = null)
        {
            var comment = new Comment
            {
                PostId = postId,
                Content = content,
                UserId = userId,
                CreatedOn = DateTime.UtcNow,
                ParentCommentId = parentCommentId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            if (parentCommentId.HasValue)
            {
                var parentComment = await _context.Comments
                    .FirstOrDefaultAsync(c => c.Id == parentCommentId.Value);

                if (parentComment != null && parentComment.UserId != userId)
                {
                    await _notificationService.CreateAsync(
                        userId: parentComment.UserId,
                        actorId: userId,
                        type: NotificationType.Reply,
                        postId: postId,
                        commentId: comment.Id);
                }
            }
            else
            {
                var post = await _context.Posts
                    .FirstOrDefaultAsync(p => p.Id == postId);

                if (post != null && post.UserId != userId)
                {
                    await _notificationService.CreateAsync(
                        userId: post.UserId,
                        actorId: userId,
                        type: NotificationType.Comment,
                        postId: postId,
                        commentId: comment.Id);
                }
            }
        }

        public async Task<bool> DeleteAsync(int commentId, string userId, bool isAdmin)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return false;
            if (comment.UserId != userId && !isAdmin) return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}