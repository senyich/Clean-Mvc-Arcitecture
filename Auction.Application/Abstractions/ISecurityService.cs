using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auction.Application.Abstractions
{
    public interface ISecurityService
    {
        Task<JwtSecurityToken> GenerateJWT(string tokenCredential, IEnumerable<Claim> claims);
        Task<JwtSecurityToken> GenerateJWT(string tokenCredential);
        bool VerifyPassword(string password, string hashedPassword);
        string HashPassword(string password);
    }

}