using Auction.Domain.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Enums;

namespace Auction.DataAccess.Repositories
{
    public class LoggerDbRepository : ILoggerService
    {
        private LoggerContext db;
        private SemaphoreSlim semaphore;
        public LoggerDbRepository(LoggerContext db)
        {
            this.db = db;
            semaphore = new SemaphoreSlim(3);
        }
        public async Task LogAsync(string sender, string message, LogState state)
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