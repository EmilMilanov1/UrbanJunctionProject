using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Interfaces;

namespace UrbanJunction.Services.Implementations
{
    public class TopicService : ITopicService
    {
        private readonly ApplicationDbContext _context;

        public TopicService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Topic>> GetAllAsync()
        {
            return await _context.Topics
                .Include(t => t.Subcategories)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Topic?> GetByNameAsync(string name)
        {
            return await _context.Topics
                .Include(t => t.Subcategories)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Name == name);
        }
    }
}