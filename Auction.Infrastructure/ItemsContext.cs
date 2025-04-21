using Microsoft.EntityFrameworkCore;
using OrderWebsite.Domain.Entities;

namespace OrderWebsite.Infrastructure
{
    public class ItemsContext : DbContext
    {
        public DbSet<ItemEntity> Items { get; set; }
        public ItemsContext(DbContextOptions<ItemsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}