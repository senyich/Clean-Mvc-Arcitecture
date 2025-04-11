using Auction.Domain.Enums;

namespace Auction.Domain.Repositories.Abstraction
{
    public interface ILoggerRepository
    {
        Task LogAsync(string sender, string message, LogState state);
    }
}

