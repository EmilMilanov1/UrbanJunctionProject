using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Interfaces;

public class TopicsController : Controller
{
    private readonly IPostService _postService;
    private readonly UserManager<UrbanUser> _userManager;

    public TopicsController(IPostService postService, UserManager<UrbanUser> userManager)
    {
        _postService = postService;
        _userManager = userManager;
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

        // Right sidebar — trending (top 4 by reaction count)
        var trending = posts
            .OrderByDescending(p => p.Reactions?.Count() ?? 0)
            .Take(4)
            .Select(p => new
            {
                p.Id,
                p.Title,
                CommentCount = p.Comments?.Count() ?? 0
            })
            .ToList();

        // Left sidebar — post counts per topic
        var artPosts = name.Equals("Art", StringComparison.OrdinalIgnoreCase) ? posts : await _postService.GetByTopicAsync("Art");
        var musicPosts = name.Equals("Music", StringComparison.OrdinalIgnoreCase) ? posts : await _postService.GetByTopicAsync("Music");
        var fashionPosts = name.Equals("Fashion", StringComparison.OrdinalIgnoreCase) ? posts : await _postService.GetByTopicAsync("Fashion");

        ViewBag.ArtCount = artPosts.Count();
        ViewBag.MusicCount = musicPosts.Count();
        ViewBag.FashionCount = fashionPosts.Count();

        // Right sidebar — member + online counts
        ViewBag.TotalMembers = _userManager.Users.Count();
        ViewBag.OnlineCount = _userManager.Users.Count(u => u.LastActiveOn >= DateTime.UtcNow.AddMinutes(-15));

        ViewBag.TotalThreads = posts.Count();
        ViewBag.TotalReplies = posts.Sum(p => p.Comments?.Count() ?? 0);
        ViewBag.TrendingPosts = trending;
        ViewBag.TopicName = name;
        ViewBag.Query = query;
        ViewBag.LikedPostIds = likedPostIds;

        return View(posts);
    }
}