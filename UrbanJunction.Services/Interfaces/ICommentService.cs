using UrbanJunction.Data.Models;

namespace UrbanJunction.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetByPostAsync(int postId);
        Task AddAsync(int postId, string content, string userId);
        Task<bool> DeleteAsync(int commentId, string userId, bool isAdmin);
    }
}
