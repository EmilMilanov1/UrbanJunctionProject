using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Interfaces;
using UrbanJunction.Data.ViewModels;

namespace UrbanJunction.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<UrbanUser> _userManager;

        public UserService(UserManager<UrbanUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ProfileViewModel> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new InvalidOperationException("User not found.");

            return new ProfileViewModel
            {
                Username = user.UserName!,
                Email = user.Email!,
                ProfilePictureUrl = user.ProfilePicturePath
            };
        }

        public async Task UpdateProfileAsync(string userId, EditProfileViewModel model)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new InvalidOperationException("User not found.");

            user.UserName = model.Username;

            if (!string.IsNullOrEmpty(model.ExistingProfilePictureUrl))
                user.ProfilePicturePath = model.ExistingProfilePictureUrl;

            await _userManager.UpdateAsync(user);
        }

        public async Task<string> UploadAvatarAsync(string userId, IFormFile file, string webRootPath)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new InvalidOperationException("User not found.");

            var uploadPath = Path.Combine(webRootPath, "uploads");
            Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            var relativePath = "/uploads/" + fileName;
            user.ProfilePicturePath = relativePath;
            await _userManager.UpdateAsync(user);

            return relativePath;
        }
    }
}
