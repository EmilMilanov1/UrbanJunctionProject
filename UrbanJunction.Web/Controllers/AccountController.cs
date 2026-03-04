using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanJunction.Data.Models;
using UrbanJunction.Web.Models;
using UrbanJunction.Data.ViewModels;
using UrbanJunction.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace UrbanJunction.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UrbanUser> _userManager;
        private readonly SignInManager<UrbanUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        private readonly IRecaptchaService _recaptchaService;
        private readonly IConfiguration _config;

        public AccountController(
            UserManager<UrbanUser> userManager,
            SignInManager<UrbanUser> signInManager,
            IWebHostEnvironment env,
            IRecaptchaService recaptchaService,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
            _recaptchaService = recaptchaService;
            _config = config;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            UrbanUser? user = null;

            if (model.UsernameOrEmail.Contains("@"))
                user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);

            if (user == null)
                user = await _userManager.FindByNameAsync(model.UsernameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var claims = new List<Claim>
                {
                    new Claim("ProfilePicturePath", user.ProfilePicturePath ?? "/images/default.jpg")
                };
                await _signInManager.SignInWithClaimsAsync(user, isPersistent: model.RememberMe, claims);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            // Pass the site key so the view can load the reCAPTCHA script
            ViewBag.RecaptchaSiteKey = _config["GoogleReCaptcha:SiteKey"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string RecaptchaToken)
        {
            ViewBag.RecaptchaSiteKey = _config["GoogleReCaptcha:SiteKey"];

            if (!ModelState.IsValid)
                return View(model);

            // Verify reCAPTCHA token before doing anything else
            var recaptchaValid = await _recaptchaService.VerifyAsync(RecaptchaToken);
            if (!recaptchaValid)
            {
                ModelState.AddModelError("", "reCAPTCHA verification failed. Please try again.");
                return View(model);
            }

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
                ProfilePicturePath = "/images/default.jpg"
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

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            return View(new ProfileViewModel
            {
                Username = user.UserName!,
                Email = user.Email!,
                ProfilePictureUrl = user.ProfilePicturePath
            });
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            return View(new EditProfileViewModel
            {
                Username = user.UserName!,
                Email = user.Email!,
                ExistingProfilePictureUrl = user.ProfilePicturePath
            });
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
                Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await model.ProfilePicture.CopyToAsync(stream);

                user.ProfilePicturePath = "/uploads/" + fileName;
            }

            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Profile));
        }
    }
}