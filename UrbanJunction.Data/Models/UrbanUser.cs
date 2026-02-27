using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UrbanJunction.Data.Models
{
    public class UrbanUser : IdentityUser
    {
        public string ProfilePicturePath { get; set; } = "/images/profile/default-profile.png";
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
