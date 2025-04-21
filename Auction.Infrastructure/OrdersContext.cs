using Microsoft.EntityFrameworkCore;
using OrderWebsite.Domain.Entities;

namespace OrderWebsite.Infrastructure
{
    public class OrdersContext : DbContext
    {
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public OrdersContext() { }
        public OrdersContext(DbContextOptions<OrdersContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderEntity>()
                .HasOne(a => a.Owner)
                .WithMany(u => u.Orders);
        }
    }
}
