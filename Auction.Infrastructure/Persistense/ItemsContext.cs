using Microsoft.EntityFrameworkCore;
using OrderWebsite.Domain.Entities;

namespace OrderWebsite.Infrastructure.Persistense
{
    public class ItemsContext : DbContext
    {
        public DbSet<ItemEntity> Items { get; set; }
        public ItemsContext(DbContextOptions<ItemsContext> options)
            : base(options)
        {
        }
    }
}