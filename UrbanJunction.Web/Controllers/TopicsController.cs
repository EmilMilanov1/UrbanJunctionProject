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
    private readonly ITagService _tagService;

    public TopicsController(
        IPostService postService,
        IReactionService reactionService,
        UserManager<UrbanUser> userManager,
        ITagService tagService)
    {
        _postService = postService;
        _reactionService = reactionService;
        _userManager = userManager;
        _tagService = tagService;
    }

    [Route("Topics/{name}")]
    public async Task<IActionResult> ByName(string name, string? query, string? sort, string? subcat, string? tag)
    {
        var posts = string.IsNullOrWhiteSpace(query)
            ? await _postService.GetByTopicAsync(name, subcat)
            : await _postService.SearchAsync(name, query);

        // Filter by tag if provided
        if (!string.IsNullOrWhiteSpace(tag))
        {
            var tagLower = tag.ToLower().TrimStart('#');
            posts = posts.Where(p =>
                p.PostTags != null &&
                p.PostTags.Any(pt => pt.Tag.Name == tagLower));
        }

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
            .OrderByDescending(p => (p.Reactions?.Count(r => r.IsUpvote) ?? 0) - (p.Reactions?.Count(r => !r.IsUpvote) ?? 0))
            .Take(4)
            .Select(p => new {
                p.Id,
                p.Title,
                Score = (p.Reactions?.Count(r => r.IsUpvote) ?? 0) - (p.Reactions?.Count(r => !r.IsUpvote) ?? 0)
            })
            .ToList();

        var artPosts = name.Equals("Art", StringComparison.OrdinalIgnoreCase) ? posts : await _postService.GetByTopicAsync("Art");
        var musicPosts = name.Equals("Music", StringComparison.OrdinalIgnoreCase) ? posts : await _postService.GetByTopicAsync("Music");
        var fashionPosts = name.Equals("Fashion", StringComparison.OrdinalIgnoreCase) ? posts : await _postService.GetByTopicAsync("Fashion");

        // Collect all tags for this topic for the filter bar
        var topicTags = posts
            .Where(p => p.PostTags != null)
            .SelectMany(p => p.PostTags.Select(pt => pt.Tag.Name))
            .GroupBy(t => t)
            .OrderByDescending(g => g.Count())
            .Take(10)
            .Select(g => g.Key)
            .ToList();

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
        ViewBag.ActiveTag = tag ?? "";
        ViewBag.TopicTags = topicTags;

        return View(posts);
    }
}