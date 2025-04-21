using Microsoft.AspNetCore.Http;

namespace OrderWebsite.Application.Abstractions
{
    public interface IFileLogisticService
    {
        Task DeleteFileAsync(string filePath, string enviromentPath);
        Task<string> SaveFileAsync(IFormFile file, string enviromentPath);
    }
}