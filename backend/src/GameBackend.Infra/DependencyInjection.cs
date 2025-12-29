using GameBackend.Core.Interfaces;
using GameBackend.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameBackend.Infra
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Handles the registration of  infrastructure services for the GameBackend
        /// and repository implementations.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="configuration">The application <see cref="IConfiguration"/> containing connection strings and settings.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with infrastructure services registered.</returns>
        public static IServiceCollection AddGameInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<GameDbContext>(options => options.UseNpgsql(connectionString));

            // Auth Tool
            services.AddScoped<IAccountRepository, AccountRepository>();

            return services;
        }
    }
}
