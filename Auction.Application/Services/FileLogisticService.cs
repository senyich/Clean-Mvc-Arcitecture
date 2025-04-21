using Microsoft.AspNetCore.Http;
using OrderWebsite.Application.Abstractions;

namespace OrderWebsite.Application.Services
{
    public class ImagesLogisticService : IFileLogisticService
    {
        private const string ImagesSubFolderPath = "UploadedImages";
        private readonly string[] extensions = new string[6] { ".png", ".jpg", ".gif", ".bmp", ".ico", ".jpeg" };
        private readonly static object locker = new object();

        public async Task<string> SaveFileAsync(IFormFile file, string enviromentPath)
        {
            string fileExtension = Path.GetExtension(file.FileName);
            if (file == null || file.Length == 0 || !extensions.Contains(fileExtension.ToLower()))
                throw new ArgumentException("Файл пустой или не соответствует формату");
            var uploadsFolderPath = Path.Combine(enviromentPath, ImagesSubFolderPath);

            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(fileStream);
            return $"/{ImagesSubFolderPath}/{uniqueFileName}";
        }
        public async Task DeleteFileAsync(string filePath, string enviromentPath)
        {
            var fullPath = Path.Combine(enviromentPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                lock(locker)
                    File.Delete(fullPath);
            }
        }
    }
}