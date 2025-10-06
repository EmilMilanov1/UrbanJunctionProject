using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanJunction.Data.Models
{
	public class Post
	{
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		public string Content { get; set; }

		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

		[Required]
		public string UserId { get; set; }
		public UrbanUser User { get; set; }

		[Required]
		public int SubcategoryId { get; set; }
		public Subcategory Subcategory { get; set; }
	}
}
