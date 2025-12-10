using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UrbanJunction.Data.Models
{
    public class UrbanUser : IdentityUser
    {
        public string ProfilePicturePath { get; set; } = "/images/profile/default-profile.png";

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
