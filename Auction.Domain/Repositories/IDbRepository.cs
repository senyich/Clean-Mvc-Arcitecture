
namespace Auction.Domain.Repositories.Abstraction
{
    public interface IDbRepository<T> where T : class
    {
        Task<int> Add(T entity);
        Task Delete(int id);
        Task Update(int id, T entity);
        Task<List<T>> GetAll();
        Task<T> Get(int id);
    }
}

