using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanJunction.Data.Models
{
	public class Topic
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } // e.g. Art, Music, Fashion

		public string Description { get; set; }

		public string ImageUrl { get; set; }

		public ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
	}
}
