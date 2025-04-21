using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using OrderWebsite.Application.Abstractions;
using OrderWebsite.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace OrderWebsite.Application.Services
{
    public class UserAuthService : IAuthService
    {
        private ISecurityService securityService;
        private IUserValidationService userValidationService;
        private IConfigurationManager config;
        public UserAuthService(
            ISecurityService securityService,
            IUserValidationService userValidationService,
            IConfigurationManager config
        )
        {
            this.config = config;
            this.userValidationService = userValidationService;
            this.securityService = securityService;
        }
        public async Task RegisterAsync(string username, string password)
        {
            var user = await userValidationService.GetSingleUserAsync(username);
            if (user != null)
                throw new Exception("Пользователь уже существует!");
            var passwordHash = securityService.HashData(password);
            var (newUser, error) = UserModel.Create(0, username, passwordHash,1000);
            if (newUser == null)
                throw new Exception(error);
            await userValidationService.CreateUserAsync(newUser);
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
            var tokenName = config["JwtTokenCode"];
            var token = securityService.GenerateEncodedJWT(tokenName, claims);
            var stringToken = new JwtSecurityTokenHandler().WriteToken(token);
            return stringToken;
        }
    }
}