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
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        // New field for the uploaded image
        public string? ImagePath { get; set; }
        public ICollection<PostImage> Images { get; set; } = new List<PostImage>();

        // existing relationships
        public int SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public UrbanUser User { get; set; } = null!;
    }

}
