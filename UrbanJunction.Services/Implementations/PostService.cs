using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Interfaces;
using UrbanJunction.Data.ViewModels;

namespace UrbanJunction.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _env;

        public PostService(
            ApplicationDbContext context,
            IImageService imageService,
            IWebHostEnvironment env)
        {
            _context = context;
            _imageService = imageService;
            _env = env;
        }

        public async Task<IEnumerable<Post>> GetByTopicAsync(string topicName)
        {
            return await _context.Posts
                .Include(p => p.Subcategory).ThenInclude(s => s.Topic)
                .Include(p => p.Images)
                .Include(p => p.User)
                .Where(p => p.Subcategory.Topic.Name == topicName)
                .OrderByDescending(p => p.CreatedOn)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> SearchAsync(string topicName, string? query)
        {
            var posts = _context.Posts
                .Include(p => p.Subcategory).ThenInclude(s => s.Topic)
                .Include(p => p.Images)
                .Include(p => p.User)
                .Where(p => p.Subcategory.Topic.Name == topicName);

            if (!string.IsNullOrWhiteSpace(query))
            {
                posts = posts.Where(p =>
                    p.Title.Contains(query) ||
                    p.Content.Contains(query) ||
                    p.User.UserName.Contains(query));
            }

            return await posts
                .OrderByDescending(p => p.CreatedOn)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Post?> GetDetailsAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Subcategory).ThenInclude(s => s.Topic)
                .Include(p => p.Images)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p => p.Reactions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post> CreateAsync(PostFormViewModel model, string userId)
        {
            var post = new Post
            {
                Title = model.Title,
                Content = model.Content,
                SubcategoryId = model.SubcategoryId,
                UserId = userId,
                CreatedOn = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                var paths = await _imageService.SaveImagesAsync(model.ImageFiles, _env.WebRootPath);
                foreach (var path in paths)
                {
                    _context.PostImages.Add(new PostImage { ImagePath = path, PostId = post.Id });
                }
                await _context.SaveChangesAsync();
            }

            return post;
        }

        public async Task<bool> EditAsync(int id, PostFormViewModel model, string userId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
            if (post == null) return false;

            post.Title = model.Title;
            post.Content = model.Content;
            post.SubcategoryId = model.SubcategoryId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string userId, bool isAdmin)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return false;
            if (post.UserId != userId && !isAdmin) return false;

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Post>> GetByUserAsync(string userId)
        {
            return await _context.Posts
                .Include(p => p.Subcategory).ThenInclude(s => s.Topic)
                .Include(p => p.Images)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedOn)
                .ToListAsync();
        }
    }
}
