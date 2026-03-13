using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Interfaces;

public class TopicsController : Controller
{
    private readonly IPostService _postService;
    private readonly IReactionService _reactionService;
    private readonly UserManager<UrbanUser> _userManager;

    public TopicsController(
        IPostService postService,
        IReactionService reactionService,
        UserManager<UrbanUser> userManager)
    {
        _postService = postService;
        _reactionService = reactionService;
        _userManager = userManager;
    }

    [Route("Topics/{name}")]
    public async Task<IActionResult> ByName(string name, string? query, string? sort, string? subcat)
    {
        var posts = string.IsNullOrWhiteSpace(query)
            ? await _postService.GetByTopicAsync(name, subcat)
            : await _postService.SearchAsync(name, query);

        posts = sort switch
        {
            "top" => posts.OrderByDescending(p => p.IsPinned)
                          .ThenByDescending(p => p.Reactions?.Count() ?? 0),
            "hot" => posts.OrderByDescending(p => p.IsPinned)
                          .ThenByDescending(p => p.Comments?.Count() ?? 0),
            _ => posts.OrderByDescending(p => p.IsPinned)
                          .ThenByDescending(p => p.CreatedOn)
        };

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userVotes = new Dictionary<int, string>();
        if (User.Identity?.IsAuthenticated == true && userId != null)
        {
            foreach (var post in posts)
            {
                var vote = await _reactionService.GetUserVoteAsync(post.Id, userId);
                if (vote != null) userVotes[post.Id] = vote;
            }
        }
        ViewBag.UserVotes = userVotes;

        var trending = posts
            .OrderByDescending(p => p.Reactions?.Count() ?? 0)
            .Take(4)
            .Select(p => new { p.Id, p.Title, CommentCount = p.Comments?.Count() ?? 0 })
            .ToList();

        var artPosts = name.Equals("Art", StringComparison.OrdinalIgnoreCase) ? posts : await _postService.GetByTopicAsync("Art");
        var musicPosts = name.Equals("Music", StringComparison.OrdinalIgnoreCase) ? posts : await _postService.GetByTopicAsync("Music");
        var fashionPosts = name.Equals("Fashion", StringComparison.OrdinalIgnoreCase) ? posts : await _postService.GetByTopicAsync("Fashion");

        ViewBag.ArtCount = artPosts.Count();
        ViewBag.MusicCount = musicPosts.Count();
        ViewBag.FashionCount = fashionPosts.Count();
        ViewBag.TotalMembers = _userManager.Users.Count();
        ViewBag.OnlineCount = _userManager.Users.Count(u => u.LastActiveOn >= DateTime.UtcNow.AddMinutes(-15));
        ViewBag.TotalThreads = posts.Count();
        ViewBag.TotalReplies = posts.Sum(p => p.Comments?.Count() ?? 0);
        ViewBag.TrendingPosts = trending;
        ViewBag.TopicName = name;
        ViewBag.Query = query;
        ViewBag.ActiveSort = sort ?? "new";
        ViewBag.ActiveSubcat = subcat ?? "all";

        return View(posts);
    }
}