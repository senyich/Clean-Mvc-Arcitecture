using Auction.Domain.Enums;

namespace Auction.Application.Abstractions
{
    public interface ILoggerService
    {
        Task LogAsync(string sender, string message, LogType logType);
    }
}