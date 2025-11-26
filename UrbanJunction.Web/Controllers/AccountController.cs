using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UrbanJunction.Data.Models;
using UrbanJunction.Web.Models;

namespace UrbanJunction.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UrbanUser> _userManager;
        private readonly SignInManager<UrbanUser> _signInManager;
        private readonly IWebHostEnvironment _env;

        public AccountController(
            UserManager<UrbanUser> userManager,
            SignInManager<UrbanUser> signInManager,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }

        // ===== AUTH =====

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            UrbanUser? user = null;

            // Try email first:
            if (model.UsernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
            }

            // If not found, try username:
            if (user == null)
            {
                user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.UserName == model.UsernameOrEmail);
            }

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                // ✅ Add the profile picture claim when signing in
                var claims = new List<Claim>
                {
                    new Claim("ProfilePictureUrl", user.ProfilePictureUrl ?? "/images/default.jpg")
                };
                await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = await _userManager.FindByNameAsync(model.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError("Username", "This username is already taken.");
                return View(model);
            }

            var user = new UrbanUser
            {
                UserName = model.Username,
                Email = model.Email,
                ProfilePictureUrl = "/images/Profile/default.jpg" // ✅ sets default on registration
            };


            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // ===== PROFILE =====

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

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
            if (user == null) return RedirectToAction("Login");

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
            if (user == null) return RedirectToAction("Login");

            if (!ModelState.IsValid)
                return View(model);

            user.UserName = model.Username;

            if (model.ProfilePicture != null)
            {
                var fileName = $"{Guid.NewGuid()}_{model.ProfilePicture.FileName}";
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await model.ProfilePicture.CopyToAsync(stream);

                user.ProfilePictureUrl = "/uploads/" + fileName;
            }

            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Profile));
        }
    }
}
