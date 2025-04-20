using Microsoft.EntityFrameworkCore;
using Auction.Domain.Repositories.Abstraction;
using Auction.Domain.Entities;

namespace Auction.Infrastructure.Repositories
{
    public class AuctionRepository : IDbRepository<OrderEntity>
    {       
        private readonly AuctionContext db;
        private SemaphoreSlim semaphore;
        public AuctionRepository(AuctionContext db)
        {
            this.db = db;
            semaphore = new SemaphoreSlim(3);
        }
        public async Task<int> Add(OrderEntity entity)
        {
            await semaphore.WaitAsync(3);
            try
            {
                await db.AuctionsLots.AddAsync(entity);
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
                await db.AuctionsLots
                    .Where(u=>u.Id == id)
                    .ExecuteDeleteAsync();
                await db.SaveChangesAsync();
            }
            catch(Exception) { throw; }
            finally { semaphore.Release(); }
        }
        public async Task<OrderEntity> Get(int id)
        {
            var auction = await db.AuctionsLots.FirstOrDefaultAsync(a => a.Id == id);
            ArgumentNullException.ThrowIfNull(auction);
            return auction;
        }
        public async Task Update(int id, OrderEntity entity)
        {
            await semaphore.WaitAsync(3);
            try
            {
                await db.AuctionsLots.Where(a=>a.Id == id)
                    .ExecuteUpdateAsync(a=>a
                        .SetProperty(a=>a.ItemId, entity.ItemId)
                        .SetProperty(a=>a.BuyPrice, entity.BuyPrice)
                        .SetProperty(a=>a.CurrentPrice, entity.CurrentPrice)
                        .SetProperty(a=>a.MinPriceUpdateRate, entity.MinPriceUpdateRate));
                await db.SaveChangesAsync();
            }
            catch(Exception) { throw; }
            finally { semaphore.Release(); }
        }
        public async Task<List<OrderEntity>> GetAll() => await db.AuctionsLots.ToListAsync();
 
    }
}
