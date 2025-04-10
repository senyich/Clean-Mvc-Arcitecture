using Microsoft.EntityFrameworkCore;
using Auction.Domain.Abstractions;
using Auction.Domain.Entities;

namespace Auction.DataAccess.Repositories
{
    public class UserRepository : IDbRepository<UserEntity>
    {
        private readonly AuctionContext db;
        private SemaphoreSlim semaphore;
        public UserRepository(AuctionContext db)
        {
            this.db = db;
            semaphore = new SemaphoreSlim(3);
        }
        public async Task<int> Add(UserEntity entity)
        {
            await semaphore.WaitAsync(3);
            try
            {
                await db.Users.AddAsync(entity);
                await db.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
            finally
            { 
                semaphore.Release();
            }
            return entity.Id;
        }
        public async Task Delete(int id)
        {
            await semaphore.WaitAsync(3);
            try
            {
                await db.Users
                    .Where(u=>u.Id == id)
                    .ExecuteDeleteAsync();
                await db.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
            finally
            { 
                semaphore.Release();
            }
        }

        public async Task<UserEntity> Get(int id)
        {
            var user = await db.Users
                .Include(a=>a.Games)
                .ThenInclude(g=>g.AuctionLot)
                .Where(a=>a.Id == id)
                .FirstOrDefaultAsync();
            ArgumentNullException.ThrowIfNull(user);
            return user;
        }
        public async Task<List<UserEntity>> GetAll() => await db.Users.ToListAsync();
        public async Task Update(int id, UserEntity entity)
        {
            await semaphore.WaitAsync(3);
            try
            {
                await db.Users.Where(u=>u.Id == id)
                    .ExecuteUpdateAsync(u=>u
                        .SetProperty(u=>u.UserName, entity.UserName)
                        .SetProperty(u=>u.PasswordHash, entity.PasswordHash));
                await db.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
            finally
            { 
                semaphore.Release();
            }
        }
    }
}


