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
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuctionEntity>()
                .HasOne(a => a.Item)
                .WithOne(g => g.AuctionLot);
            modelBuilder.Entity<ItemEntity>()
                .HasOne(i => i.Owner)
                .WithMany(u => u.Items);
            modelBuilder.Entity<AuctionEntity>()
                .HasOne(a => a.Owner)
                .WithMany(u => u.AuctionLots);
        }
    }
}
