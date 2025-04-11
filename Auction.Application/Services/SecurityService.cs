using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auction.Application.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace Auction.Application.Services
{
    public class SecurityService : ISecurityService
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 500;
        public async Task<JwtSecurityToken> GenerateJWT(string tokenCredential, IEnumerable<Claim> claims)
        {
            var credentialEncode = Encoding.UTF8.GetBytes(tokenCredential);
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(credentialEncode),
                SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(24),
                claims: claims
            );
            return token;
        }

        public async Task<JwtSecurityToken> GenerateJWT(string tokenCredential)
        {
            var credentialEncode = Encoding.UTF8.GetBytes(tokenCredential);
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(credentialEncode),
                SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(24)
            );
            return token;
        }

        public string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }
        public bool VerifyPassword(string password, string hashedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);

            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var hash = new byte[HashSize];
            Array.Copy(hashBytes, SaltSize, hash, 0, HashSize);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            var testHash = pbkdf2.GetBytes(HashSize);
            
            for (int i = 0; i < HashSize; i++)
            {
                if (hash[i] != testHash[i])
                    return false;
            }
            return true;
        }
    }
}