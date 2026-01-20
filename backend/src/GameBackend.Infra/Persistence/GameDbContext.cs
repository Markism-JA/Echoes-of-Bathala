using GameBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameBackend.Infra.Persistence
{
    public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
    {
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameDbContext).Assembly);
        }
    }
}
