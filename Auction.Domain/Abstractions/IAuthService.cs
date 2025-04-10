
namespace Auction.Domain.Abstractions
{
    public interface IAuthService
    {
        Task<bool> Login(string username, string password);
        Task<int> Register(string username, string password);
    }
}