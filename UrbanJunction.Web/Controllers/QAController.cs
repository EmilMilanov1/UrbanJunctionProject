using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanJunction.Data;
using UrbanJunction.Data.Models;

namespace UrbanJunction.Web.Controllers
{
    [Authorize]
    public class QAController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UrbanUser> _userManager;

        public QAController(ApplicationDbContext context, UserManager<UrbanUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.Name = user?.UserName ?? "";
            ViewBag.Email = user?.Email ?? "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(string name, string email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(message))
            {
                TempData["Error"] = "Subject and message are required.";
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            _context.ContactMessages.Add(new ContactMessage
            {
                SenderId = userId,
                Name = name,
                Email = email,
                Subject = subject,
                Message = message
            });

            await _context.SaveChangesAsync();
            TempData["Success"] = "Message sent. We'll get back to you soon.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkRead(int id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg != null)
            {
                msg.IsRead = true;
                await _context.SaveChangesAsync();
            }
            TempData["Success"] = "Message marked as read.";
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg != null)
            {
                _context.ContactMessages.Remove(msg);
                await _context.SaveChangesAsync();
            }
            TempData["Success"] = "Message deleted.";
            return RedirectToAction("Index", "Admin");
        }
    }
}