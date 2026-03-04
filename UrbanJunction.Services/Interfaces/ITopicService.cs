using UrbanJunction.Data.Models;

namespace UrbanJunction.Services.Interfaces
{
    public interface ITopicService
    {
        Task<IEnumerable<Topic>> GetAllAsync();
        Task<Topic?> GetByNameAsync(string name);
    }
}