using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using UrbanJunction.Data;
using UrbanJunction.Data.Models;
using UrbanJunction.Data.ViewModels;
using UrbanJunction.Services.Interfaces;
using UrbanJunction.Web.Models;

namespace UrbanJunction.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UrbanUser> _userManager;
        private readonly SignInManager<UrbanUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        private readonly IRecaptchaService _recaptchaService;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private readonly IReactionService _reactionService;

        public AccountController(
            UserManager<UrbanUser> userManager,
            SignInManager<UrbanUser> signInManager,
            IWebHostEnvironment env,
            IRecaptchaService recaptchaService,
            IConfiguration config,
            ApplicationDbContext context,
            IReactionService reactionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
            _recaptchaService = recaptchaService;
            _config = config;
            _context = context;
            _reactionService = reactionService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

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
            ViewBag.RecaptchaSiteKey = _config["GoogleReCaptcha:SiteKey"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string RecaptchaToken)
        {
            ViewBag.RecaptchaSiteKey = _config["GoogleReCaptcha:SiteKey"];
            if (!ModelState.IsValid) return View(model);

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

            return await BuildProfileViewModel(user.Id, user.Id);
        }

        [HttpGet]
        [Route("u/{username}")]
        public async Task<IActionResult> UserProfile(string username)
        {
            var profileUser = await _userManager.FindByNameAsync(username);
            if (profileUser == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await BuildProfileViewModel(profileUser.Id, currentUserId);
        }

        private async Task<IActionResult> BuildProfileViewModel(string profileUserId, string? currentUserId)
        {
            var user = await _userManager.FindByIdAsync(profileUserId);
            if (user == null) return NotFound();

            var posts = await _context.Posts
                .Include(p => p.Subcategory).ThenInclude(s => s.Topic)
                .Include(p => p.Images)
                .Include(p => p.Reactions)
                .Include(p => p.Comments)
                .Where(p => p.UserId == user.Id)
                .OrderByDescending(p => p.CreatedOn)
                .ToListAsync();

            var likedPosts = await _context.Posts
                .Include(p => p.Subcategory).ThenInclude(s => s.Topic)
                .Include(p => p.Images)
                .Include(p => p.Reactions)
                .Include(p => p.Comments)
                .Where(p => p.Reactions.Any(r => r.UserId == user.Id))
                .OrderByDescending(p => p.CreatedOn)
                .ToListAsync();

            var followers = await _context.UserFollows
                .Include(f => f.Follower)
                .Where(f => f.FollowingId == user.Id)
                .Select(f => new ProfileUserDto
                {
                    Username = f.Follower.UserName!,
                    ProfilePictureUrl = f.Follower.ProfilePicturePath
                }).ToListAsync();

            var following = await _context.UserFollows
                .Include(f => f.Following)
                .Where(f => f.FollowerId == user.Id)
                .Select(f => new ProfileUserDto
                {
                    Username = f.Following.UserName!,
                    ProfilePictureUrl = f.Following.ProfilePicturePath
                }).ToListAsync();

            var isFollowed = currentUserId != null &&
                await _context.UserFollows.AnyAsync(f => f.FollowerId == currentUserId && f.FollowingId == user.Id);

            // Build userVotes for the viewing user (not the profile owner)
            var userVotes = new Dictionary<int, string>();
            if (currentUserId != null)
            {
                var allPosts = posts.Concat(likedPosts).DistinctBy(p => p.Id);
                foreach (var post in allPosts)
                {
                    var vote = await _reactionService.GetUserVoteAsync(post.Id, currentUserId);
                    if (vote != null) userVotes[post.Id] = vote;
                }
            }
            ViewBag.UserVotes = userVotes;

            var vm = new ProfileViewModel
            {
                UserId = user.Id,
                Username = user.UserName!,
                Email = user.Email!,
                ProfilePictureUrl = user.ProfilePicturePath,
                BannerImageUrl = user.BannerImagePath,
                Bio = user.Bio,
                FollowerCount = followers.Count,
                FollowingCount = following.Count,
                IsFollowedByCurrentUser = isFollowed,
                IsOwnProfile = currentUserId == user.Id,
                Followers = followers,
                FollowingUsers = following,
                Posts = posts,
                LikedPosts = likedPosts
            };

            return View("Profile", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFollow(string targetUserId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null || currentUserId == targetUserId)
                return BadRequest();

            var existing = await _context.UserFollows
                .FirstOrDefaultAsync(f => f.FollowerId == currentUserId && f.FollowingId == targetUserId);

            if (existing != null)
                _context.UserFollows.Remove(existing);
            else
                _context.UserFollows.Add(new UserFollow { FollowerId = currentUserId, FollowingId = targetUserId });

            await _context.SaveChangesAsync();

            var targetUser = await _userManager.FindByIdAsync(targetUserId);
            return RedirectToAction("UserProfile", new { username = targetUser!.UserName });
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
                Bio = user.Bio,
                ExistingProfilePictureUrl = user.ProfilePicturePath,
                ExistingBannerImageUrl = user.BannerImagePath
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model, string? CroppedProfilePicture, string? CroppedBannerImage)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            if (!ModelState.IsValid) return View(model);

            user.UserName = model.Username;
            user.Bio = model.Bio;

            var uploadPath = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadPath);

            if (!string.IsNullOrEmpty(CroppedProfilePicture))
            {
                user.ProfilePicturePath = SaveBase64Image(CroppedProfilePicture, uploadPath);
            }
            else if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.ProfilePicture.FileName)}";
                var filePath = Path.Combine(uploadPath, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await model.ProfilePicture.CopyToAsync(stream);
                user.ProfilePicturePath = "/uploads/" + fileName;
            }

            if (!string.IsNullOrEmpty(CroppedBannerImage))
            {
                user.BannerImagePath = SaveBase64Image(CroppedBannerImage, uploadPath);
            }
            else if (model.BannerImage != null && model.BannerImage.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.BannerImage.FileName)}";
                var filePath = Path.Combine(uploadPath, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await model.BannerImage.CopyToAsync(stream);
                user.BannerImagePath = "/uploads/" + fileName;
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction(nameof(Profile));
        }

        private string SaveBase64Image(string base64, string uploadPath)
        {
            try
            {
                string base64Data = base64.Contains(",") ? base64.Split(',')[1] : base64;
                base64Data = base64Data.Trim().Replace(" ", "+").Replace("\n", "").Replace("\r", "");
                int mod = base64Data.Length % 4;
                if (mod > 0) base64Data += new string('=', 4 - mod);

                var bytes = Convert.FromBase64String(base64Data);
                var fileName = $"{Guid.NewGuid()}.jpg";
                var filePath = Path.Combine(uploadPath, fileName);
                System.IO.File.WriteAllBytes(filePath, bytes);
                return "/uploads/" + fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveBase64Image error: " + ex.Message);
                return "/images/default.jpg";
            }
        }
    }
}