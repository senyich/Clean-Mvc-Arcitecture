using Microsoft.EntityFrameworkCore;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Repositories;

namespace OrderWebsite.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OrdersContext db;
        private SemaphoreSlim semaphore;
        public UserRepository(OrdersContext db)
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
                .FirstOrDefaultAsync(a => a.Id == id);
            ArgumentNullException.ThrowIfNull(user);
            return user;
        }
        public async Task<List<UserEntity>> GetAll() => await db.Users.ToListAsync();

        public async Task<UserEntity> GetSingleUserByUsername(string username)
        {
            var user = await db.Users
                .Include(a=>a.Orders)
                .FirstOrDefaultAsync(a => a.UserName == username);
            ArgumentNullException.ThrowIfNull(user);
            return user;
        }
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


