using Microsoft.AspNetCore.Http;

namespace Auction.Domain.Abstractions
{
    public interface IFileLogisticService
    {
        Task DeleteFileAsync(string filePath, string enviromentPath);
        Task<string> SaveFileAsync(IFormFile file, string enviromentPath);
    }
}