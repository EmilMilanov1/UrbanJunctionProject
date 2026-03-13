using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data;
using UrbanJunction.Services.Interfaces;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UrbanUser> _userManager;

    public AdminController(
        IAdminService adminService,
        ApplicationDbContext context,
        UserManager<UrbanUser> userManager)
    {
        _adminService = adminService;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var stats = await _adminService.GetStatsAsync();
        return View(stats);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePost(int id)
    {
        var success = await _adminService.DeletePostAsync(id);
        if (!success) return NotFound();

        TempData["Success"] = "Post deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TogglePin(int id, string? returnUrl)
    {
        var post = await _context.Posts
            .Include(p => p.Subcategory).ThenInclude(s => s.Topic)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (post == null) return NotFound();
        post.IsPinned = !post.IsPinned;
        await _context.SaveChangesAsync();
        TempData["Success"] = post.IsPinned ? "Post pinned." : "Post unpinned.";
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return RedirectToAction("ByName", "Topics", new { name = post.Subcategory?.Topic?.Name ?? "Art" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleLock(int id, string? returnUrl)
    {
        var post = await _context.Posts
            .Include(p => p.Subcategory).ThenInclude(s => s.Topic)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (post == null) return NotFound();
        post.IsLocked = !post.IsLocked;
        await _context.SaveChangesAsync();
        TempData["Success"] = post.IsLocked ? "Post locked." : "Post unlocked.";
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return RedirectToAction("ByName", "Topics", new { name = post.Subcategory?.Topic?.Name ?? "Art" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BanUser(string userId)
    {
        var success = await _adminService.BanUserAsync(userId);
        if (!success) return NotFound();

        TempData["Success"] = "User banned.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnbanUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        await _userManager.SetLockoutEndDateAsync(user, null);

        TempData["Success"] = "User unbanned.";
        return RedirectToAction(nameof(Index));
    }
}