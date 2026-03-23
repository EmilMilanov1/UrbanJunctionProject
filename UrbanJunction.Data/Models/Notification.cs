using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanJunction.Data.Models
{
    public enum NotificationType
    {
        Follow,
        Reaction,
        Comment,
        Reply,
        AdminReply
    }

    public class Notification
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public UrbanUser User { get; set; } = null!;

        public string? ActorId { get; set; }
        public UrbanUser? Actor { get; set; }

        public NotificationType Type { get; set; }

        public int? PostId { get; set; }
        public Post? Post { get; set; }

        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }

        public int? ContactMessageId { get; set; }
        public ContactMessage? ContactMessage { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}