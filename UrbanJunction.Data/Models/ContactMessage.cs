namespace UrbanJunction.Data.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = null!;
        public UrbanUser Sender { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? AdminReply { get; set; }
        public DateTime? RepliedOn { get; set; }
    }
}