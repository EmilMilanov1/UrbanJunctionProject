using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanJunction.Services.Interfaces;

public class SearchController : Controller
{
    private readonly IPostService _postService;
    private readonly IReactionService _reactionService;

    public SearchController(IPostService postService, IReactionService reactionService)
    {
        _postService = postService;
        _reactionService = reactionService;
    }

    public async Task<IActionResult> Index(string? query, string? topic, string? sort)
    {
        var posts = await _postService.SearchAllAsync(query, topic, sort);

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
        ViewBag.Query = query ?? "";
        ViewBag.ActiveTopic = topic ?? "all";
        ViewBag.ActiveSort = sort ?? "new";
        ViewBag.ResultCount = posts.Count();

        return View(posts);
    }
}