using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanJunction.Data;
using UrbanJunction.Data.Models;

[Authorize]
public class ReportsController : Controller
{
    private readonly ApplicationDbContext _context;
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(
        ReportReason reason,
        int? postId,
        int? commentId,
        string? reportedUserId,
        string? returnUrl)
    {
        // Prevent duplicate pending reports
        var duplicate = _context.Reports.Any(r =>
            r.ReporterId == UserId &&
            r.Status == ReportStatus.Pending &&
            r.PostId == postId &&
            r.CommentId == commentId &&
            r.ReportedUserId == reportedUserId);

        if (!duplicate)
        {
            _context.Reports.Add(new Report
            {
                ReporterId = UserId,
                Reason = reason,
                PostId = postId,
                CommentId = commentId,
                ReportedUserId = reportedUserId,
                CreatedOn = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            TempData["Success"] = "Report submitted.";
        }
        else
        {
            TempData["Info"] = "You have already reported this.";
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Resolve(int id)
    {
        var report = await _context.Reports.FindAsync(id);
        if (report == null) return NotFound();
        report.Status = ReportStatus.Resolved;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Report resolved.";
        return RedirectToAction("Index", "Admin");
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Dismiss(int id)
    {
        var report = await _context.Reports.FindAsync(id);
        if (report == null) return NotFound();
        report.Status = ReportStatus.Dismissed;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Report dismissed.";
        return RedirectToAction("Index", "Admin");
    }
}