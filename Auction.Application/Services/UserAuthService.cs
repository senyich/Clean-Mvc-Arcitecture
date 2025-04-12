using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Auction.Application.Abstractions;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class UserAuthService : IAuthService
    {
        private ISecurityService securityService;
        private IUserValidationService userValidationService;
        public UserAuthService(
            ISecurityService securityService,
            IUserValidationService userValidationService
        )
        {
            this.userValidationService = userValidationService;
            this.securityService = securityService;
        }
        public async Task RegisterAsync(string username, string password)
        {
            var user = await userValidationService.GetSingleUserAsync(username);
            if (user != null)
                throw new Exception("Пользователь уже существует!");
            var passwordHash = securityService.HashData(password);
            var (newUser, error) = UserModel.Create(0, username, passwordHash);
            if (newUser == null)
                throw new Exception(error);
            await userValidationService.AddUserAsync(newUser);
        }
        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await userValidationService.GetSingleUserAsync(username);
            if (user == null)
                throw new Exception("Пользователя с таким именем не существует!");
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            //по хорошему нужно это имя хранить в засекреченном формате
            var tokenName = $"younkyounkyounkyounkyounkyounkyounkyounkyounk";
            var token = await securityService.GenerateEncodedJWT(tokenName, claims);
            var stringToken = new JwtSecurityTokenHandler().WriteToken(token);
            return stringToken;
        }
    }
}