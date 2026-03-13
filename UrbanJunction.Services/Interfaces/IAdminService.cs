using UrbanJunction.Data.Models;

namespace UrbanJunction.Services.Interfaces
{
    public class AdminStatsViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalPosts { get; set; }
        public int TotalTopics { get; set; }
        public int TotalComments { get; set; }
        public int OnlineUsers { get; set; }
        public int PendingReportCount { get; set; }
        public int UnreadMessageCount { get; set; }


        public IEnumerable<Post> RecentPosts { get; set; } = new List<Post>();
        public IEnumerable<UrbanUser> AllUsers { get; set; } = new List<UrbanUser>();
        public IEnumerable<Topic> AllTopics { get; set; } = new List<Topic>();
        public IEnumerable<Report> PendingReports { get; set; } = new List<Report>();
        public IEnumerable<ContactMessage> ContactMessages { get; set; } = new List<ContactMessage>();

    }

    public interface IAdminService
    {
        Task<AdminStatsViewModel> GetStatsAsync();
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<IEnumerable<UrbanUser>> GetAllUsersAsync();
        Task<bool> DeletePostAsync(int id);
        Task<bool> BanUserAsync(string userId);
    }
}