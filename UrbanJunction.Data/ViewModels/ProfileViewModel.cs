namespace UrbanJunction.Data.ViewModels
{
    public class ProfileViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public string? BannerImageUrl { get; set; }
        public IEnumerable<UrbanJunction.Data.Models.Post> Posts { get; set; } = new List<UrbanJunction.Data.Models.Post>();
        public IEnumerable<UrbanJunction.Data.Models.Post> LikedPosts { get; set; } = new List<UrbanJunction.Data.Models.Post>();

    }
}