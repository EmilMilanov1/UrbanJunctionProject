using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanJunction.Data;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TopicsController : Controller
{
    private readonly IPostService _postService;
    private readonly IReactionService _reactionService;
    private readonly UserManager<UrbanUser> _userManager;
    private readonly ITagService _tagService;
    private readonly ApplicationDbContext _context;

    public TopicsController(
        IPostService postService,
        IReactionService reactionService,
        UserManager<UrbanUser> userManager,
        ITagService tagService,
        ApplicationDbContext context)
    {
        _postService = postService;
        _reactionService = reactionService;
        _userManager = userManager;
        _tagService = tagService;
        _context = context;
    }

    [Route("Topics/{name}")]
    public async Task<IActionResult> ByName(string name, string? query, string? sort, string? subcat, string? tag)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var posts = string.IsNullOrWhiteSpace(query)
            ? await _postService.GetByTopicAsync(name, subcat)
            : await _postService.SearchAsync(name, query);

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

        var postList = posts.ToList();

        // Single query for all user votes
        var userVotes = new Dictionary<int, string>();
        if (User.Identity?.IsAuthenticated == true && userId != null)
        {
            var postIds = postList.Select(p => p.Id).ToList();
            var reactions = await _context.Reactions
                .AsNoTracking()
                .Where(r => r.UserId == userId && postIds.Contains(r.PostId))
                .Select(r => new { r.PostId, r.IsUpvote })
                .ToListAsync();

            foreach (var r in reactions)
                userVotes[r.PostId] = r.IsUpvote ? "up" : "down";
        }

        // Sequential count queries — lightweight, no full post loads
        var artCount = await _context.Posts.AsNoTracking().CountAsync(p => p.Subcategory.Topic.Name == "Art");
        var musicCount = await _context.Posts.AsNoTracking().CountAsync(p => p.Subcategory.Topic.Name == "Music");
        var fashionCount = await _context.Posts.AsNoTracking().CountAsync(p => p.Subcategory.Topic.Name == "Fashion");
        var totalMembers = await _context.Users.AsNoTracking().CountAsync();
        var onlineCount = await _context.Users.AsNoTracking()
            .CountAsync(u => u.LastActiveOn >= DateTime.UtcNow.AddMinutes(-15));

        var trending = postList
            .OrderByDescending(p =>
                (p.Reactions?.Count(r => r.IsUpvote) ?? 0) -
                (p.Reactions?.Count(r => !r.IsUpvote) ?? 0))
            .Take(4)
            .Select(p => new {
                p.Id,
                p.Title,
                Score = (p.Reactions?.Count(r => r.IsUpvote) ?? 0) -
                        (p.Reactions?.Count(r => !r.IsUpvote) ?? 0)
            })
            .ToList();

        var topicTags = postList
            .Where(p => p.PostTags != null)
            .SelectMany(p => p.PostTags.Select(pt => pt.Tag.Name))
            .GroupBy(t => t)
            .OrderByDescending(g => g.Count())
            .Take(10)
            .Select(g => g.Key)
            .ToList();

        ViewBag.UserVotes = userVotes;
        ViewBag.ArtCount = artCount;
        ViewBag.MusicCount = musicCount;
        ViewBag.FashionCount = fashionCount;
        ViewBag.TotalMembers = totalMembers;
        ViewBag.OnlineCount = onlineCount;
        ViewBag.TotalThreads = postList.Count;
        ViewBag.TotalReplies = postList.Sum(p => p.Comments?.Count() ?? 0);
        ViewBag.TrendingPosts = trending;
        ViewBag.TopicName = name;
        ViewBag.Query = query;
        ViewBag.ActiveSort = sort ?? "new";
        ViewBag.ActiveSubcat = subcat ?? "all";
        ViewBag.ActiveTag = tag ?? "";
        ViewBag.TopicTags = topicTags;

        return View(postList);
    }
}