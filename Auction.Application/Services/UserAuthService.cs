using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Auction.Domain.Abstractions;
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
        public async Task Register(string username, string password)
        {
            var user = (await userValidationService.GetUsersAsync()).FirstOrDefault(u => u.UserName == username);
            if (user != null)
                throw new Exception("Пользователь уже существует!");
            var passwordHash = securityService.HashPassword(password);
            var (newUser, error) = UserModel.Create(0, username, passwordHash);
            if (newUser == null)
                throw new Exception(error);
            await userValidationService.AddUserAsync(newUser);
        }
        public async Task<JwtSecurityToken> Login(string username, string password)
        {
            var user = (await userValidationService.GetUsersAsync()).FirstOrDefault(u => u.UserName == username);
            if (user == null)
                throw new Exception("Пользователя с таким именем не существует!");
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var tokenName = $"younkyounkyounkyounkyounkyounkyounkyounkyounk";
            return await securityService.GenerateJWT(tokenName, claims);
        }
    }
}