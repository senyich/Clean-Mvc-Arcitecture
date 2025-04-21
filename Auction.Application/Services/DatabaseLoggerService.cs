using OrderWebsite.Application.Abstractions;
using OrderWebsite.Domain.Enums;
using OrderWebsite.Domain.Repositories.Abstraction;

namespace OrderWebsite.Application.Services
{
    public class DatabaseLoggerService : ILoggerService
    {
        private ILoggerRepository loggerDbRepository;
        public DatabaseLoggerService(ILoggerRepository loggerDbRepository)
        {
            this.loggerDbRepository = loggerDbRepository;
        }
        public async Task LogAsync(string sender, string message, LogType logType)
            => await loggerDbRepository.AddLoggedData(sender, message, logType);

    }
}
