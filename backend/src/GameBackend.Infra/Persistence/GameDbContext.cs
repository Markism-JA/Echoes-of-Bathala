using GameBackend.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameBackend.Infra.Persistence;

public class GameDbContext(DbContextOptions<GameDbContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<RefreshToken> refreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<InventoryItem>();
        modelBuilder.Ignore<PlayerCharacter>();
        modelBuilder.Ignore<QuestProgress>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameDbContext).Assembly);
    }
}
