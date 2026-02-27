namespace UrbanJunction.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public int PostId { get; set; }
        public Post Post { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public UrbanUser User { get; set; } = null!;
    }
}
