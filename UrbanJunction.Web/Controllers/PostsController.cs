using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UrbanJunction.Data;
using UrbanJunction.Data.Models;

[Authorize]
public class PostsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PostsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult MyPosts()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var posts = _context.Posts
            .Include(p => p.Subcategory)
                .ThenInclude(sc => sc.Topic)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedOn)
            .ToList();

        return View(posts);
    }

    [Authorize]
    public IActionResult Create()
    {
        var model = new PostFormViewModel
        {
            Subcategories = _context.Subcategories
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Topic.Name + " / " + s.Name
                }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create(PostFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Subcategories = _context.Subcategories
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Topic.Name + " / " + s.Name
                }).ToList();

            return View(model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var post = new Post
        {
            Title = model.Title,
            Content = model.Content,
            SubcategoryId = model.SubcategoryId,
            UserId = userId,
            CreatedOn = DateTime.UtcNow
        };

        _context.Posts.Add(post);
        _context.SaveChanges();

        TempData["Success"] = "Your post has been created!";
        return RedirectToAction("MyPosts");
    }
    [Authorize]
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var post = _context.Posts
            .FirstOrDefault(p => p.Id == id && p.UserId == userId);

        if (post == null)
        {
            return NotFound();
        }

        _context.Posts.Remove(post);
        _context.SaveChanges();

        TempData["Success"] = "Post deleted successfully.";
        return RedirectToAction("MyPosts");
    }
    [Authorize]
    public IActionResult Edit(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var post = _context.Posts
            .Include(p => p.Subcategory)
            .ThenInclude(s => s.Topic)
            .FirstOrDefault(p => p.Id == id && p.UserId == userId);

        if (post == null)
        {
            return NotFound();
        }

        var model = new PostFormViewModel
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            SubcategoryId = post.SubcategoryId,
            Subcategories = _context.Subcategories
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Topic.Name + " / " + s.Name
                }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Edit(int id, PostFormViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var post = _context.Posts.FirstOrDefault(p => p.Id == id && p.UserId == userId);

        if (post == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.Subcategories = _context.Subcategories
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Topic.Name + " / " + s.Name
                }).ToList();

            return View(model);
        }

        post.Title = model.Title;
        post.Content = model.Content;
        post.SubcategoryId = model.SubcategoryId;

        _context.SaveChanges();

        TempData["Success"] = "Post updated successfully!";
        return RedirectToAction("MyPosts");
    }
    [AllowAnonymous]
    public IActionResult Details(int id)
    {
        var post = _context.Posts
            .Include(p => p.User)
            .Include(p => p.Subcategory)
            .ThenInclude(s => s.Topic)
            .FirstOrDefault(p => p.Id == id);

        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }


}
