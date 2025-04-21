using Microsoft.EntityFrameworkCore;
using OrderWebsite.Domain.Repositories.Abstraction;
using OrderWebsite.Domain.Entities;

namespace OrderWebsite.Infrastructure.Repositories
{
    public class ItemRepository : IDbRepository<ItemEntity>
    {
        private readonly ItemsContext db;
        private SemaphoreSlim semaphore;
        public ItemRepository(ItemsContext db)
        {
            this.db = db;
            semaphore = new SemaphoreSlim(3);
        }
        public async Task<int> Add(ItemEntity entity)
        {
            await semaphore.WaitAsync(3);
            try
            {
                await db.Items.AddAsync(entity);
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
                await db.Items
                    .Where(u=>u.Id == id)
                    .ExecuteDeleteAsync();
                await db.SaveChangesAsync();
            }
            catch(Exception) { throw; }
            finally { semaphore.Release(); }
        }

        public async Task<ItemEntity> Get(int id)
        {
            var game = await db.Items
                .FirstOrDefaultAsync(a => a.Id == id);
            ArgumentNullException.ThrowIfNull(game);
            return game;
        }
        public async Task Update(int id, ItemEntity entity)
        {
            await semaphore.WaitAsync(3);
            try
            {
                await db.Items.Where(g=>g.Id == id)
                    .ExecuteUpdateAsync(g=>g
                        .SetProperty(g=>g.OrderId, entity.OrderId)
                        .SetProperty(g=>g.Description, entity.Description)
                        .SetProperty(g=>g.ImgPath, entity.ImgPath)
                        .SetProperty(g=>g.Name, entity.Name));
                await db.SaveChangesAsync();
            }
            catch(Exception) { throw; }
            finally { semaphore.Release(); }
        }
        public async Task<List<ItemEntity>> GetAll() => await db.Items.ToListAsync();
    }
}

