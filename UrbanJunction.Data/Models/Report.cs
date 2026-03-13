using System;

namespace UrbanJunction.Data.Models
{
    public enum ReportReason
    {
        Spam,
        Harassment,
        HateSpeech,
        Misinformation,
        InappropriateContent
    }

    public enum ReportStatus
    {
        Pending,
        Resolved,
        Dismissed
    }

    public class Report
    {
        public int Id { get; set; }
        public string ReporterId { get; set; } = null!;
        public UrbanUser Reporter { get; set; } = null!;
        public ReportReason Reason { get; set; }
        public ReportStatus Status { get; set; } = ReportStatus.Pending;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        // Only one of these will be set
        public int? PostId { get; set; }
        public Post? Post { get; set; }

        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }

        public string? ReportedUserId { get; set; }
        public UrbanUser? ReportedUser { get; set; }
    }
}