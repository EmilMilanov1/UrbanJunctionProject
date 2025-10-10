using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace UrbanJunction.Web.ViewModels.Account
{
    public class EditProfileViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public IFormFile? ProfilePicture { get; set; }
        public string? ExistingProfilePictureUrl { get; set; }
    }
}
