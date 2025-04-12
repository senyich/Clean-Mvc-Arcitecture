using Auction.Application.Abstractions;
using Auction.Domain.Enums;
using Auction.Domain.Repositories.Abstraction;

namespace Auction.Application.Services
{
    public class DatabaseLoginService : ILoggerService
    {
        private ILoggerRepository loggerDbRepository;
        public DatabaseLoginService(ILoggerRepository loggerDbRepository)
        {
            this.loggerDbRepository = loggerDbRepository;
        }
        public async Task LogAsync(string sender, string message, LogType logType)
            => await loggerDbRepository.AddLoggedData(sender, message, logType);

    }
}
