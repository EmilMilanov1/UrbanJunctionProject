namespace UrbanJunction.Data.ViewModels
{
    public class ProfileUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
    }

    public class ProfileViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public string? BannerImageUrl { get; set; }
        public string? Bio { get; set; }

        public int FollowerCount { get; set; }
        public int FollowingCount { get; set; }
        public bool IsFollowedByCurrentUser { get; set; }
        public bool IsOwnProfile { get; set; }

        public IEnumerable<ProfileUserDto> Followers { get; set; } = new List<ProfileUserDto>();
        public IEnumerable<ProfileUserDto> FollowingUsers { get; set; } = new List<ProfileUserDto>();

        public IEnumerable<UrbanJunction.Data.Models.Post> Posts { get; set; } = new List<UrbanJunction.Data.Models.Post>();
        public IEnumerable<UrbanJunction.Data.Models.Post> LikedPosts { get; set; } = new List<UrbanJunction.Data.Models.Post>();
    }
}