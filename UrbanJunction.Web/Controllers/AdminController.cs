using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data;

namespace UrbanJunction.Web.Controllers
{
    [Authorize(Roles = "Admin")]  // ✅ Only admins can access
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin Dashboard
        public async Task<IActionResult> Index()
        {
            var stats = new
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalPosts = await _context.Posts.CountAsync(),
                TotalTopics = await _context.Topics.CountAsync()
            };

            return View(stats);
        }

        // View all posts (for moderation)
        public async Task<IActionResult> AllPosts()
        {
            var posts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Subcategory)
                .ThenInclude(s => s.Topic)
                .OrderByDescending(p => p.CreatedOn)
                .ToListAsync();

            return View(posts);
        }

        // Delete any post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Post deleted successfully.";
            return RedirectToAction(nameof(AllPosts));
        }
    }
}