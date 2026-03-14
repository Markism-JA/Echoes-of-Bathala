using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Echoes.Infrastructure.Persistence.Postgresql;

/// <summary>
/// Facilitates the creation of <see cref="GameDbContext"/> instances during design-time
/// (e.g., when running Entity Framework Migrations).
/// </summary>
public class GameDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
{
    /// <summary>
    /// Creates a new DB context by reading the connection string from the API's appsettings.json.
    /// </summary>
    public GameDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Echoes.API");

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseNpgsql(connectionString);

        return new GameDbContext(optionsBuilder.Options);
    }
}
