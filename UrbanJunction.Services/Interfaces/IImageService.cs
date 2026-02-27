using Microsoft.AspNetCore.Http;

namespace UrbanJunction.Services.Interfaces
{
    public interface IImageService
    {
        Task<List<string>> SaveImagesAsync(List<IFormFile> files, string webRootPath);
        void DeleteImage(string imagePath, string webRootPath);
    }
}
