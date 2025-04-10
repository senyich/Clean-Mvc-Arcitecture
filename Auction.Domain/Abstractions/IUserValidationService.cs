using Auction.Domain.Models;

namespace Auction.Domain.Abstractions
{
    public interface IUserValidationService
    {
        Task<int> AddUserAsync(UserModel user);
        Task<UserModel> GetUserAsync(int id);
        Task<List<UserModel>> GetUsersAsync();
        Task RemoveUserAsync(int id);
        Task UpdateUserAsync(int id, UserModel user);
    }
}