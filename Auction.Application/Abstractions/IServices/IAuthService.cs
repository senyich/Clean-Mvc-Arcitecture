namespace OrderWebsite.Application.Abstractions.IServices
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);
        Task RegisterAsync(string username, string password);
    }
}