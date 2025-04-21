
using Microsoft.EntityFrameworkCore;
using OrderWebsite.Domain;
using OrderWebsite.Domain.Entities;
namespace OrderWebsite.Infrastructure
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