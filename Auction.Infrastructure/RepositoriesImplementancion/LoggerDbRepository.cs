using OrderWebsite.Domain.Repositories;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Enums;

namespace OrderWebsite.Infrastructure.Repositories
{
    public class LoggerDbRepository : ILoggerRepository
    {
        private LoggerContext db;
        private SemaphoreSlim semaphore;
        public LoggerDbRepository(LoggerContext db)
        {
            this.db = db;
            semaphore = new SemaphoreSlim(3);
        }
        public async Task AddLoggedData(string sender, string message, LogType state)
        {
            await semaphore.WaitAsync();
            try
            {
                Console.WriteLine($"[{sender}]: {message} at {DateTime.UtcNow} ({state})");
                var logData = new LogDataEntity();
                logData.Sender = sender;
                logData.Message = message;
                logData.TypeOfMessage = state.ToString();
                logData.Time = DateTime.UtcNow;
                await db.LogData.AddAsync(logData);
                await db.SaveChangesAsync();   
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}