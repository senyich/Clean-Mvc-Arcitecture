using Microsoft.AspNetCore.Http;
using Auction.Domain.Abstractions;

namespace Auction.Application.Services
{
    public class ImagesLogisticService : IFileLogisticService
    {
        private const string imagesSubFolderPath = "UploadedImages";
        public async Task<string> SaveFileAsync(IFormFile file, string enviromentPath)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Файл пустой");
            var uploadsFolderPath = Path.Combine(enviromentPath, imagesSubFolderPath);

            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(fileStream);

            return $"/{imagesSubFolderPath}/{uniqueFileName}";
        }
        public Task DeleteFileAsync(string filePath, string enviromentPath)
        {
            var fullPath = Path.Combine(enviromentPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            return Task.CompletedTask;
        }

    }
}