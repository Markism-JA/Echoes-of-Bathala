using Echoes.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Echoes.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<GameDbContext>(options => options.UseNpgsql(connectionString));
            return services;
        }

        public static IServiceCollection AddPersistenceWorkerInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services;
        }

        public static IServiceCollection AddCryptoWorkerInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services;
        }

        public static IServiceCollection AddGameServerInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services;
        }
    }
}
