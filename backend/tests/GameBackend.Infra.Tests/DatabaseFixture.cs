using GameBackend.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace GameBackend.Infra.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;

    public DatabaseFixture()
    {
        _dbContainer = new PostgreSqlBuilder("postgres:17-alpine")
            .WithDatabase("game_backend_test_db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    public GameDbContext CreateContext()
    {
        var connectionString = _dbContainer.GetConnectionString();
        var options = new DbContextOptionsBuilder<GameDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        var context = new GameDbContext(options);

        context.Database.EnsureCreated();

        return context;
    }
}
