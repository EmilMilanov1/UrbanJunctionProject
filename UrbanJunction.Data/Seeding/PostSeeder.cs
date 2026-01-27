using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanJunction.Data.Models;

namespace UrbanJunction.Data.Seeding
{
	public class PostSeeder
	{
		public static IEnumerable<Post> SeedPosts()
		{
			IEnumerable<Post> posts = new List<Post>()
			{
				new Post
				{
					Id = 1,
					Title = "Best Graffiti Spots in Berlin",
					Content = "Check out the East Side Gallery and RAW Gelände!",
					CreatedOn = new DateTime(2026, 1, 19),
					UserId =  "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",          
					SubcategoryId = 1           // Graffiti
				},
				new Post
				{
					Id = 2,
					Title = "Underground Techno in Detroit",
					Content = "The scene is raw and authentic. Worth experiencing!",
					CreatedOn = DateTime.UtcNow,
					UserId =  "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
					SubcategoryId = 2           // Techno
				},
				new Post
				{
					Id = 3,
					Title = "Streetwear Trends for 2025",
					Content = "Baggy is back. Sneakers are getting chunkier than ever.",
					CreatedOn = DateTime.UtcNow,
					UserId =  "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
					SubcategoryId = 3           // Streetwear
				}
			};

			return posts;
		}
	}
}
