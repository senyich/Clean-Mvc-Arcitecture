using Auction.Domain.Models;

namespace Auction.Application.Abstractions
{
    public interface IUserValidationService
    {
        Task<int> CreateUserAsync(UserModel user);
        Task<UserModel?> GetSingleUserAsync(int id);
        Task<UserModel?> GetSingleUserAsync(string usename);
        Task<List<UserModel>> GetAllUsersAsync();
        Task RemoveUserAsync(int id);
        Task UpdateUserAsync(int id, UserModel user);
    }
}