using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Interfaces;

namespace UrbanJunction.Services.Implementations
{
    public class ReactionService : IReactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public ReactionService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<int> ToggleAsync(int postId, string userId)
        {
            var existing = await _context.Reactions
                .FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);

            if (existing != null)
                _context.Reactions.Remove(existing);
            else
                _context.Reactions.Add(new Reaction
                {
                    PostId = postId,
                    UserId = userId,
                    IsUpvote = true,
                    CreatedOn = DateTime.UtcNow
                });

            await _context.SaveChangesAsync();
            return await GetCountAsync(postId);
        }

        public async Task<int> GetCountAsync(int postId)
        {
            return await _context.Reactions.CountAsync(r => r.PostId == postId);
        }

        public async Task<bool> HasUserLikedAsync(int postId, string userId)
        {
            return await _context.Reactions
                .AnyAsync(r => r.PostId == postId && r.UserId == userId);
        }

        public async Task VoteAsync(int postId, string userId, bool isUpvote)
        {
            var existing = await _context.Reactions
                .FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);

            bool isNewUpvote = false;

            if (existing != null)
            {
                if (existing.IsUpvote == isUpvote)
                    _context.Reactions.Remove(existing);
                else
                    existing.IsUpvote = isUpvote;
            }
            else
            {
                _context.Reactions.Add(new Reaction
                {
                    PostId = postId,
                    UserId = userId,
                    IsUpvote = isUpvote,
                    CreatedOn = DateTime.UtcNow
                });

                if (isUpvote) isNewUpvote = true;
            }

            await _context.SaveChangesAsync();

            if (isNewUpvote)
            {
                var post = await _context.Posts
                    .FirstOrDefaultAsync(p => p.Id == postId);

                if (post != null && post.UserId != userId)
                {
                    await _notificationService.CreateAsync(
                        userId: post.UserId,
                        actorId: userId,
                        type: NotificationType.Reaction,
                        postId: postId);
                }
            }
        }

        public async Task<int> GetScoreAsync(int postId)
        {
            var upvotes = await _context.Reactions.CountAsync(r => r.PostId == postId && r.IsUpvote);
            var downvotes = await _context.Reactions.CountAsync(r => r.PostId == postId && !r.IsUpvote);
            return upvotes - downvotes;
        }

        public async Task<string?> GetUserVoteAsync(int postId, string userId)
        {
            var reaction = await _context.Reactions
                .FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);

            if (reaction == null) return null;
            return reaction.IsUpvote ? "up" : "down";
        }
    }
}