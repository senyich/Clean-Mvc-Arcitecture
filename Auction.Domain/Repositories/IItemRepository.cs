using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Repositories.Abstraction;

namespace OrderWebsite.Domain.Repositories
{
    public interface IItemRepository : IDbRepository<ItemEntity>
    {
    }
}
