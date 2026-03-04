using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanJunction.Services.Interfaces;

public class TopicsController : Controller
{
    private readonly IPostService _postService;

    public TopicsController(IPostService postService)
    {
        _postService = postService;
    }

    [Route("Topics/{name}")]
    public async Task<IActionResult> ByName(string name, string? query)
    {
        var posts = string.IsNullOrWhiteSpace(query)
            ? await _postService.GetByTopicAsync(name)
            : await _postService.SearchAsync(name, query);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var likedPostIds = new HashSet<int>();

        if (userId != null)
        {
            likedPostIds = posts
                .Where(p => p.Reactions.Any(r => r.UserId == userId))
                .Select(p => p.Id)
                .ToHashSet();
        }

        ViewBag.TopicName = name;
        ViewBag.Query = query;
        ViewBag.LikedPostIds = likedPostIds;

        return View(posts);
    }
}