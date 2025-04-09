using Microsoft.EntityFrameworkCore;
using Auction.Domain.Abstractions;
using Auction.Domain.Entities;

namespace Auction.DataAccess.Repositories
{
    public class GameRepository : IDbRepository<GameEntity>
    {
        private readonly AuctionContext db;
        private SemaphoreSlim semaphore;
        public GameRepository(AuctionContext db)
        {
            this.db = db;
            semaphore = new SemaphoreSlim(3);
        }
        public async Task<int> Add(GameEntity entity)
        {
            await semaphore.WaitAsync(3);
            try
            {
                await db.Games.AddAsync(entity);
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
                await db.Games
                    .Where(u=>u.Id == id)
                    .ExecuteDeleteAsync();
                await db.SaveChangesAsync();
            }
            catch(Exception) { throw; }
            finally { semaphore.Release(); }
        }

        public async Task<GameEntity> Get(int id)
        {
            var game = await db.Games
                .Include(a=>a.AuctionLot)
                .FirstOrDefaultAsync(a => a.Id == id);
            ArgumentNullException.ThrowIfNull(game);
            return game;
        }
        public async Task Update(int id, GameEntity entity)
        {
            await semaphore.WaitAsync(3);
            try
            {
                await db.Games.Where(g=>g.Id == id)
                    .ExecuteUpdateAsync(g=>g
                        .SetProperty(g=>g.AuctionId, entity.AuctionId)
                        .SetProperty(g=>g.Description, entity.Description)
                        .SetProperty(g=>g.ImgPath, entity.ImgPath)
                        .SetProperty(g=>g.Name, entity.Name));
                await db.SaveChangesAsync();
            }
            catch(Exception) { throw; }
            finally { semaphore.Release(); }
        }
        public async Task<List<GameEntity>> GetAll() => await db.Games.ToListAsync();
    }
}

