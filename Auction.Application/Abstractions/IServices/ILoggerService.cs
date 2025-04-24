using OrderWebsite.Domain.Enums;

namespace OrderWebsite.Application.Abstractions.IServices
{
    public interface ILoggerService
    {
        Task LogAsync(string sender, string message, LogType logType);
    }
}