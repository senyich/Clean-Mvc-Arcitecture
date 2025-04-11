
using System.IdentityModel.Tokens.Jwt;

namespace Auction.Application.Abstractions
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> Login(string username, string password);
        Task Register(string username, string password);
    }
}