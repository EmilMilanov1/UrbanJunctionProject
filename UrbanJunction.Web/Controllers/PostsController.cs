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
    public async Task<IActionResult> Create(PostFormViewModel model, List<IFormFile>? imageFiles)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var post = new Post
        {
            Title = model.Title,
            Content = model.Content,
            SubcategoryId = model.SubcategoryId,
            UserId = userId!
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync(); // we need Post.Id before saving images

        if (imageFiles != null && imageFiles.Count > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            foreach (var file in imageFiles)
            {
                if (file.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var postImage = new PostImage
                    {
                        ImagePath = "/uploads/" + uniqueFileName,
                        PostId = post.Id
                    };

                    _context.PostImages.Add(postImage);
                }
            }

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("ByName", "Topics", new { name = post.Subcategory.Topic.Name });
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
