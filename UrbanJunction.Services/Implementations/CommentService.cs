using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Interfaces;

namespace UrbanJunction.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
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

        public async Task AddAsync(int postId, string content, string userId)
        {
            var comment = new Comment
            {
                Content = content,
                PostId = postId,
                UserId = userId,
                CreatedOn = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
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
