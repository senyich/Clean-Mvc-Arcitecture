using Microsoft.EntityFrameworkCore;
using Auction.Domain.Abstractions;
using Auction.Domain.Entities;

namespace Auction.DataAccess.Repositories
{
    public class AuctionRepository : IDbRepository<AuctionEntity>
    {       
        private readonly AuctionContext db;
        private SemaphoreSlim semaphore;
        public AuctionRepository(AuctionContext db)
        {
            this.db = db;
            semaphore = new SemaphoreSlim(3);
        }
        public async Task<int> Add(AuctionEntity entity)
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
        public async Task<AuctionEntity> Get(int id)
        {
            var auction = await db.AuctionsLots.FirstOrDefaultAsync(a => a.Id == id);
            ArgumentNullException.ThrowIfNull(auction);
            return auction;
        }
        public async Task Update(int id, AuctionEntity entity)
        {
            await semaphore.WaitAsync(3);
            try
            {
                await db.AuctionsLots.Where(a=>a.Id == id)
                    .ExecuteUpdateAsync(a=>a
                        .SetProperty(a=>a.GameId, entity.GameId)
                        .SetProperty(a=>a.BuyPrice, entity.BuyPrice)
                        .SetProperty(a=>a.CurrentPrice, entity.CurrentPrice)
                        .SetProperty(a=>a.MinPriceUpdateRate, entity.MinPriceUpdateRate));
                await db.SaveChangesAsync();
            }
            catch(Exception) { throw; }
            finally { semaphore.Release(); }
        }
        public async Task<List<AuctionEntity>> GetAll() => await db.AuctionsLots.ToListAsync();
 
    }
}
