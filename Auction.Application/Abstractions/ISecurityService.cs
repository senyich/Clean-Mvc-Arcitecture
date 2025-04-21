using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OrderWebsite.Application.Abstractions
{
    public interface ISecurityService
    {
        JwtSecurityToken GenerateEncodedJWT(string tokenCredential, IEnumerable<Claim> claims);
        JwtSecurityToken GenerateEncodedJWT(string tokenCredential);
        bool VerifyHashedData(string data, string hashedHashed);
        string HashData(string data);
    }

}