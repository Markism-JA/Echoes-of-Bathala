using GameBackend.Core.Interfaces;
using GameBackend.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameBackend.Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddGameInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<GameDbContext>(options => options.UseNpgsql(connectionString));

            //Auth Tool
            services.AddScoped<IAccountRepository, AccountRepository>();

            return services;
        }
    }
}
