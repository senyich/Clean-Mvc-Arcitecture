using Auction.Domain.Enums;

namespace Auction.Domain.Abstractions
{
    public interface ILoggerService
    {
        Task LogAsync(string sender, string message, LogState state);
    }
}

