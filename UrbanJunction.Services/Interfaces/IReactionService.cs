namespace UrbanJunction.Services.Interfaces
{
    public interface IReactionService
    {
        Task<int> ToggleAsync(int postId, string userId);
        Task<int> GetCountAsync(int postId);
        Task<bool> HasUserLikedAsync(int postId, string userId);
    }
}
