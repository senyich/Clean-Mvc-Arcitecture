
using Microsoft.EntityFrameworkCore;
using Auction.Domain;
using Auction.Domain.Entities;
namespace Auction.DataAccess
{
    public class LoggerContext : DbContext
    {
        public DbSet<LogDataEntity> LogData { get; set; }
        public LoggerContext(DbContextOptions<LoggerContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}