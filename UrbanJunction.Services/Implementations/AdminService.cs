using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Interfaces;

namespace UrbanJunction.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UrbanUser> _userManager;

        public AdminService(ApplicationDbContext context, UserManager<UrbanUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AdminStatsViewModel> GetStatsAsync()
        {
            var recentPosts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Subcategory).ThenInclude(s => s.Topic)
                .Include(p => p.Reactions)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedOn)
                .Take(10)
                .ToListAsync();

            var allUsers = await _context.Users
                .Cast<UrbanUser>()
                .ToListAsync();

            var allTopics = await _context.Topics
                .Include(t => t.Subcategories)
                    .ThenInclude(s => s.Posts)
                .ToListAsync();
            var pendingReports = await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.Post)
                .Include(r => r.Comment)
                .Include(r => r.ReportedUser)
                .Where(r => r.Status == ReportStatus.Pending)
                .OrderByDescending(r => r.CreatedOn)
                .ToListAsync();
            var contactMessages = await _context.ContactMessages
                .Include(m => m.Sender)
                .OrderByDescending(m => m.CreatedOn)
                .ToListAsync();
            
            return new AdminStatsViewModel
            {
                TotalUsers = allUsers.Count,
                TotalPosts = await _context.Posts.CountAsync(),
                TotalTopics = await _context.Topics.CountAsync(),
                TotalComments = await _context.Comments.CountAsync(),
                OnlineUsers = allUsers.Count(u => u.LastActiveOn >= DateTime.UtcNow.AddMinutes(-15)),
                RecentPosts = recentPosts,
                AllUsers = allUsers,
                AllTopics = allTopics,
                PendingReports = pendingReports,
                PendingReportCount = pendingReports.Count,
                ContactMessages = contactMessages,
                UnreadMessageCount = contactMessages.Count(m => !m.IsRead)
            };
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Subcategory).ThenInclude(s => s.Topic)
                .Include(p => p.Reactions)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<UrbanUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return false;

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BanUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            return true;
        }
    }
}