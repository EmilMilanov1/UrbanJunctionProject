using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanJunction.Data.Models;

namespace UrbanJunction.Data.Seeding
{
	public class TopicSeeder
	{
		public static IEnumerable<Topic> SeedTopics()
		{

			IEnumerable<Topic> topics = new List<Topic>()
			{
				new Topic
				{
					Id = 1,
					Name = "Art",
					Description = "Everything about art",
					ImageUrl = "/img/art.jpg"
				},
				new Topic
				{
					Id = 2,
					Name = "Music",
					Description = "All genres of music",
					ImageUrl = "/img/music.jpg"
				},
				new Topic
				{
					Id = 3,
					Name = "Fashion",
					Description = "Trends, streetwear, and design",
					ImageUrl = "/img/fashion.jpg"
				}
			};

			return topics;
		}
		
	}
}
