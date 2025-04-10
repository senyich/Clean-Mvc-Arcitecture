using System.IdentityModel.Tokens.Jwt;

namespace Auction.Domain.Abstractions
{
    public interface ISecurityService
    {
        Task<JwtSecurityToken> GenerateJWT(string tokenCredential);
        bool VerifyPassword(string password, string hashedPassword);
        string HashPassword(string password);
    }

}