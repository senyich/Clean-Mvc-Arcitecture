
namespace OrderWebsite.Domain.Repositories.Abstraction
{
    public interface IDbRepository<Entity> where Entity : class
    {
        Task<int> Add(Entity entity);
        Task Delete(int id);
        Task Update(int id, Entity entity);
        Task<IEnumerable<Entity>> GetAll();
        Task<Entity> Get(int id);
    }
}

