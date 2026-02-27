using UrbanJunction.Data.Models;

namespace UrbanJunction.Services.Interfaces
{
    public class AdminStatsViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalPosts { get; set; }
        public int TotalTopics { get; set; }
        public int TotalComments { get; set; }
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
