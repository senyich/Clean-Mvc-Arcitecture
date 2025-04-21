using OrderWebsite.Domain.Models;

namespace OrderWebsite.Application.Abstractions
{
    public interface IUserValidationService
    {
        Task<int> CreateUserAsync(UserModel user);
        Task<UserModel?> GetSingleUserAsync(int id);
        Task<UserModel?> GetSingleUserAsync(string username);
        Task<List<UserModel>> GetAllUsersAsync();
        Task RemoveUserAsync(int id);
        Task UpdateUserAsync(int id, UserModel newUser);
    }
}