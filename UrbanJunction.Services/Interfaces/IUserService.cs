using Microsoft.AspNetCore.Http;
using UrbanJunction.Data.ViewModels;

namespace UrbanJunction.Services.Interfaces
{
    public interface IUserService
    {
        Task<ProfileViewModel> GetProfileAsync(string userId);
        Task UpdateProfileAsync(string userId, EditProfileViewModel model);
        Task<string> UploadAvatarAsync(string userId, IFormFile file, string webRootPath);
    }
}