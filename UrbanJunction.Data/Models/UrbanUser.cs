using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanJunction.Data.Models
{
	public class UrbanUser : IdentityUser
	{
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public string ProfilePictureUrl { get; set; } = "/images/default.jpg";
    }
}
