using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auction.Application.Abstractions
{
    public interface ISecurityService
    {
        Task<JwtSecurityToken> GenerateEncodedJWT(string tokenCredential, IEnumerable<Claim> claims);
        Task<JwtSecurityToken> GenerateEncodedJWT(string tokenCredential);
        bool VerifyHashedData(string data, string hashedHashed);
        string HashData(string data);
    }

}