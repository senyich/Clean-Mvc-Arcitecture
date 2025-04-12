using Auction.Domain.Enums;

namespace Auction.Domain.Repositories.Abstraction
{
    public interface ILoggerRepository
    {
        Task AddLoggedData(string sender, string message, LogType state);
    }
}

