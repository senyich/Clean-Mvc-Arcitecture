using OrderWebsite.Domain.Enums;

namespace OrderWebsite.Domain.Repositories
{
    public interface ILoggerRepository
    {
        Task AddLoggedData(string sender, string message, LogType state);
    }
}

