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
            return new AdminStatsViewModel
            {
                TotalUsers    = await _context.Users.CountAsync(),
                TotalPosts    = await _context.Posts.CountAsync(),
                TotalTopics   = await _context.Topics.CountAsync(),
                TotalComments = await _context.Comments.CountAsync()
            };
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Subcategory).ThenInclude(s => s.Topic)
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
