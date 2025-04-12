using System.IdentityModel.Tokens.Jwt;

namespace Auction.Application.Abstractions
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);
        Task RegisterAsync(string username, string password);
    }
}