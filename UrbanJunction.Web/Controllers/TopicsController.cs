using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data;

public class TopicsController : Controller
{
    private readonly ApplicationDbContext _context;

    public TopicsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // /Topics/ByName?name=Art
    [Route("Topics/{name}")]
    public IActionResult ByName(string name)
    {
        var posts = _context.Posts
            .Include(p => p.Subcategory)
            .ThenInclude(s => s.Topic)
            .Where(p => p.Subcategory.Topic.Name == name)
            .ToList();

        ViewBag.TopicName = name;
        return View(posts);
    }

}
