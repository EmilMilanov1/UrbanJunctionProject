using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UrbanJunction.Data;
using UrbanJunction.Data.ViewModels;
using UrbanJunction.Services.Interfaces;

[Authorize]
public class PostsController : Controller
{
    private readonly IPostService _postService;
    private readonly IReactionService _reactionService;
    private readonly ICommentService _commentService;
    private readonly ApplicationDbContext _context;

    public PostsController(
        IPostService postService,
        IReactionService reactionService,
        ICommentService commentService,
        ApplicationDbContext context)
    {
        _postService = postService;
        _reactionService = reactionService;
        _commentService = commentService;
        _context = context;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    private bool IsAdmin => User.IsInRole("Admin");

    public async Task<IActionResult> MyPosts()
    {
        var posts = await _postService.GetByUserAsync(UserId);
        return View(posts);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new PostFormViewModel
        {
            Subcategories = GetSubcategoryList()
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PostFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Subcategories = GetSubcategoryList();
            return View(model);
        }

        var post = await _postService.CreateAsync(model, UserId);

        var subcategory = await _context.Subcategories
            .Include(s => s.Topic)
            .FirstOrDefaultAsync(s => s.Id == model.SubcategoryId);

        TempData["Success"] = "Post created successfully!";
        return RedirectToAction("ByName", "Topics", new { name = subcategory?.Topic?.Name ?? "Art" });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var post = await _postService.GetDetailsAsync(id);

        if (post == null || (post.UserId != UserId && !IsAdmin))
            return NotFound();

        var model = new PostFormViewModel
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            SubcategoryId = post.SubcategoryId,
            Subcategories = GetSubcategoryList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PostFormViewModel model)
    {
        if (id != model.Id) return BadRequest();

        if (!ModelState.IsValid)
        {
            model.Subcategories = GetSubcategoryList();
            return View(model);
        }

        var success = await _postService.EditAsync(id, model, UserId);
        if (!success) return NotFound();

        TempData["Success"] = "Post updated successfully!";
        return RedirectToAction(nameof(MyPosts));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _postService.DeleteAsync(id, UserId, IsAdmin);
        if (!success) return NotFound();

        TempData["Success"] = "Post deleted successfully.";
        return RedirectToAction(nameof(MyPosts));
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var post = await _postService.GetDetailsAsync(id);
        if (post == null) return NotFound();

        var likeCount = await _reactionService.GetCountAsync(id);
        var userLiked = User.Identity?.IsAuthenticated == true
            ? await _reactionService.HasUserLikedAsync(id, UserId)
            : false;

        ViewBag.LikeCount = likeCount;
        ViewBag.UserLiked = userLiked;

        return View(post);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleLike(int id)
    {
        await _reactionService.ToggleAsync(id, UserId);
        var count = await _reactionService.GetCountAsync(id);
        var userLiked = await _reactionService.HasUserLikedAsync(id, UserId);

        // Always return JSON regardless of how the request was made
        return Json(new { count, userLiked });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int id, string content)
    {
        if (!string.IsNullOrWhiteSpace(content))
            await _commentService.AddAsync(id, content, UserId);

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteComment(int commentId, int postId)
    {
        await _commentService.DeleteAsync(commentId, UserId, IsAdmin);
        return RedirectToAction(nameof(Details), new { id = postId });
    }


    private List<SelectListItem> GetSubcategoryList()
    {
        return _context.Subcategories
            .Include(s => s.Topic)
            .Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = $"{s.Topic.Name} / {s.Name}"
            }).ToList();
    }
}