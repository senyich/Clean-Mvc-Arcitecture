using Auction.Domain.Repositories.Abstraction;
using Auction.Domain.Entities;

namespace Auction.Domain.Repositories
{
    public interface IUserRepository : IDbRepository<UserEntity>
    {
        Task<UserEntity> GetSingleUserByUsername(string username);
    }
}
