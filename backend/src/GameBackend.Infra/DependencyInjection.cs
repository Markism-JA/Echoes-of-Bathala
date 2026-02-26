using GameBackend.Core.Common.Authentication;
using GameBackend.Core.Entities;
using GameBackend.Core.Interfaces.Persistence;
using GameBackend.Core.Interfaces.Repository;
using GameBackend.Core.Interfaces.Security;
using GameBackend.Core.Interfaces.Services;
using GameBackend.Core.Services;
using GameBackend.Infra.Authentication;
using GameBackend.Infra.Persistence;
using GameBackend.Infra.Persistence.Repositories;
using GameBackend.Infra.Security;
using GameBackend.Infra.Services;
using Microsoft.AspNetCore.Identity;
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

            services
                .AddIdentityCore<User>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 1;
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<GameDbContext>()
                .AddDefaultTokenProviders();
            services.AddDataProtection();
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IPasswordPolicy, PasswordPolicy>();
            services.AddScoped<IUsernamePolicy, UsernamePolicy>();
            services.AddScoped<IEmailPolicy, EmailPolicy>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            return services;
        }
        // NOTE: This would get complex given that there would be cases where certain services are only needed in specific projects.
        // This should be broken down into logical SOC. In example separate Method for those in API.
    }
}
