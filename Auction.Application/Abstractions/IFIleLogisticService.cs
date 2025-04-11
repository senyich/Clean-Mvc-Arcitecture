using Microsoft.AspNetCore.Http;

namespace Auction.Application.Abstractions
{
    public interface IFileLogisticService
    {
        Task DeleteFileAsync(string filePath, string enviromentPath);
        Task<string> SaveFileAsync(IFormFile file, string enviromentPath);
    }
}