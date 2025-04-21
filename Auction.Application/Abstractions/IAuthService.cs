
namespace OrderWebsite.Application.Abstractions
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);
        Task RegisterAsync(string username, string password);
    }
}