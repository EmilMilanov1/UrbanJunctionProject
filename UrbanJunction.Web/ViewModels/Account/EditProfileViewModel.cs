using Microsoft.AspNetCore.Http;

namespace UrbanJunction.Web.ViewModels.Account
{
    public class EditProfileViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IFormFile? ProfilePicture { get; set; }
        public string? ExistingProfilePictureUrl { get; set; }
    }
}
