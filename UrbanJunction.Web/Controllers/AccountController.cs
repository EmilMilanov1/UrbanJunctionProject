using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanJunction.Data.Models;
using UrbanJunction.Web.ViewModels.Account;

namespace UrbanJunction.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UrbanUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public AccountController(UserManager<UrbanUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login", new { area = "Identity" });

            var model = new ProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login", new { area = "Identity" });

            var model = new EditProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                ExistingProfilePictureUrl = user.ProfilePictureUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (!ModelState.IsValid) return View(model);

            user.UserName = model.Username;

            if (model.ProfilePicture != null)
            {
                var fileName = $"{Guid.NewGuid()}_{model.ProfilePicture.FileName}";
                var path = Path.Combine(_env.WebRootPath, "uploads", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePictureUrl = "/uploads/" + fileName;
            }

            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Profile));
        }
    }
}
