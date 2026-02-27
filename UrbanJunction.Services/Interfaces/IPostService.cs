using UrbanJunction.Data.Models;
using UrbanJunction.Data.ViewModels;

namespace UrbanJunction.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetByTopicAsync(string topicName);
        Task<IEnumerable<Post>> SearchAsync(string topicName, string? query);
        Task<Post?> GetDetailsAsync(int id);
        Task<Post> CreateAsync(PostFormViewModel model, string userId);
        Task<bool> EditAsync(int id, PostFormViewModel model, string userId);
        Task<bool> DeleteAsync(int id, string userId, bool isAdmin);
        Task<IEnumerable<Post>> GetByUserAsync(string userId);
    }
}
