using Microsoft.AspNetCore.Http;
using UrbanJunction.Services.Interfaces;

namespace UrbanJunction.Services.Implementations
{
    public class ImageService : IImageService
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        public async Task<List<string>> SaveImagesAsync(List<IFormFile> files, string webRootPath)
        {
            var savedPaths = new List<string>();
            var uploadsFolder = Path.Combine(webRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            foreach (var file in files)
            {
                if (file.Length == 0) continue;
                if (file.Length > MaxFileSizeBytes) continue;

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(ext)) continue;

                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(stream);

                savedPaths.Add("/uploads/" + fileName);
            }

            return savedPaths;
        }

        public void DeleteImage(string imagePath, string webRootPath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;

            var fullPath = Path.Combine(webRootPath, imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
