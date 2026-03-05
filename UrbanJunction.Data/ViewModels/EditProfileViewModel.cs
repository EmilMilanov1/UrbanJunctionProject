using Microsoft.AspNetCore.Http;

public class EditProfileViewModel
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public IFormFile? ProfilePicture { get; set; }
    public string? ExistingProfilePictureUrl { get; set; }
    public IFormFile? BannerImage { get; set; }
    public string? ExistingBannerImageUrl { get; set; }
}