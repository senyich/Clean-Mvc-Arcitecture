using Microsoft.EntityFrameworkCore;
using OrderWebsite.Domain.Entities;

namespace OrderWebsite.Infrastructure.Persistense
{
    public class TradingExchangeContext : DbContext
    {
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public TradingExchangeContext() { }
        public TradingExchangeContext(DbContextOptions<TradingExchangeContext> options)
            : base(options)
        {

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
