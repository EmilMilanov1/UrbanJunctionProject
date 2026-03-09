namespace UrbanJunction.Services.Interfaces
{
    public interface IReactionService
    {
        Task<int> ToggleAsync(int postId, string userId);
        Task<int> GetCountAsync(int postId);
        Task<bool> HasUserLikedAsync(int postId, string userId);
        Task VoteAsync(int postId, string userId, bool isUpvote);
        Task<int> GetScoreAsync(int postId);
        Task<string?> GetUserVoteAsync(int postId, string userId);
    }
}