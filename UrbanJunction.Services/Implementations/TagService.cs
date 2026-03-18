using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Implementations;

public class TagService : ITagService
{
    private readonly ApplicationDbContext _context;
    public TagService(ApplicationDbContext context) => _context = context;

    public async Task<Tag> GetOrCreateAsync(string name)
    {
        name = name.Trim().ToLower().TrimStart('#');
        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == name);
        if (tag == null)
        {
            tag = new Tag { Name = name };
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }
        return tag;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync() =>
        await _context.Tags.ToListAsync();
}