using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanJunction.Data.Models;

namespace UrbanJunction.Data.Seeding
{
	public class SubcategorySeeder
	{
		public static IEnumerable<Subcategory> SeedSubcategories()
		{
			IEnumerable<Subcategory> subcategories = new List<Subcategory>()
			{
				new Subcategory
				{
					Id = 1,
					Name = "Graffiti",
					TopicId = 1 // Art
				},
				new Subcategory
				{
					Id = 2,
					Name = "Techno",
					TopicId = 2 // Music
				},
				new Subcategory
				{
					Id = 3,
					Name = "Streetwear",
					TopicId = 3 // Fashion
				}
			};


			return subcategories;
		}
	}
}
