using OrderWebsite.Domain.Repositories.Abstraction;
using OrderWebsite.Domain.Entities;

namespace OrderWebsite.Domain.Repositories
{
    public interface IUserRepository : IDbRepository<UserEntity>
    {
        Task<UserEntity> GetSingleUserByUsername(string username);
    }
}
