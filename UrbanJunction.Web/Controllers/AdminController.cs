using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanJunction.Services.Interfaces;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public async Task<IActionResult> Index()
    {
        var stats = await _adminService.GetStatsAsync();
        return View(stats);
    }

    public async Task<IActionResult> AllPosts()
    {
        var posts = await _adminService.GetAllPostsAsync();
        return View(posts);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePost(int id)
    {
        var success = await _adminService.DeletePostAsync(id);
        if (!success) return NotFound();

        TempData["Success"] = "Post deleted successfully.";
        return RedirectToAction(nameof(AllPosts));
    }
}