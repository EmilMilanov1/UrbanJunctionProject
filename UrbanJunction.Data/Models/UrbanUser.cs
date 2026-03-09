using Microsoft.AspNetCore.Identity;
using UrbanJunction.Data.Models;

public class UrbanUser : IdentityUser
{
    public string ProfilePicturePath { get; set; } = "/images/default.jpg";
    public string? BannerImagePath { get; set; }
    public string? Bio { get; set; }
    public DateTime LastActiveOn { get; set; } = DateTime.UtcNow;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();
    public ICollection<UserFollow> Following { get; set; } = new List<UserFollow>();
}