using Microsoft.EntityFrameworkCore;
using Auction.Domain.Entities;

namespace Auction.Infrastructure
{
    public class AuctionContext : DbContext
    {
        public DbSet<AuctionEntity> AuctionsLots { get; set; }
        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public AuctionContext() { }
        public AuctionContext(DbContextOptions<AuctionContext> options)
            : base(options)
        { 
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseNpgsql("Host=62.113.107.207;Port=5432;DataBase=Auction;Username=senya;Password=animenit2002");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuctionEntity>()
                .HasOne(a => a.Item)
                .WithOne(g => g.AuctionLot);
        }
    }
}
