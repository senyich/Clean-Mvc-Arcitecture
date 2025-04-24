using Microsoft.EntityFrameworkCore;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Repositories;
using OrderWebsite.Infrastructure.Persistense;

namespace OrderWebsite.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {       
        private readonly TradingExchangeContext db;
        private SemaphoreSlim semaphore;
        public OrderRepository(TradingExchangeContext db)
        {
            this.db = db;
            semaphore = new SemaphoreSlim(3);
        }
        public async Task<int> Add(OrderEntity entity)
        {
            await semaphore.WaitAsync(3);
            try
            {
                await db.Orders.AddAsync(entity);
                await db.SaveChangesAsync();
            }
            catch(Exception) { throw; }
            finally { semaphore.Release(); }
            return entity.Id;
        }
        public async Task Delete(int id)
        {
            await semaphore.WaitAsync(3);
            try
            {
                await db.Orders
                    .Where(u=>u.Id == id)
                    .ExecuteDeleteAsync();
                await db.SaveChangesAsync();
            }
            catch(Exception) { throw; }
            finally { semaphore.Release(); }
        }
        public async Task<OrderEntity> Get(int id)
        {
            var order = await db.Orders
                .FirstOrDefaultAsync(a => a.Id == id);
            ArgumentNullException.ThrowIfNull(order);
            return order;
        }
        public async Task Update(int id, OrderEntity entity)
        {
            await semaphore.WaitAsync(3);
            try
            {
                await db.Orders.Where(a => a.Id == id)
                    .ExecuteUpdateAsync(a => a
                        .SetProperty(a => a.ItemId, entity.ItemId)
                        .SetProperty(a => a.OwnerId, entity.OwnerId)
                        .SetProperty(a => a.BuyPrice, entity.BuyPrice));
                await db.SaveChangesAsync();
            }
            catch(Exception) { throw; }
            finally { semaphore.Release(); }
        }
        public async Task<IEnumerable<OrderEntity>> GetAll()
        {
            var orders = await db.Orders.ToListAsync();
            return orders != null ? orders : new List<OrderEntity>();
        }
    }
}
