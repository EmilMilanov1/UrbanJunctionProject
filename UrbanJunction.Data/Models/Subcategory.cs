using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanJunction.Data.Models
{
	public class Subcategory
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } // e.g. Graffiti, Jazz, Streetwear

		[Required]
		public int TopicId { get; set; }
		public Topic Topic { get; set; }

		public ICollection<Post> Posts { get; set; } = new List<Post>();
	}
}
