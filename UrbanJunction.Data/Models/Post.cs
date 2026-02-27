namespace UrbanJunction.Data.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public ICollection<PostImage> Images { get; set; } = new List<PostImage>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

        public int SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public UrbanUser User { get; set; } = null!;
    }

}
