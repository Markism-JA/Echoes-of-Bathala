using System.Text;
using Echoes.Application.Common.Identity;
using Echoes.Application.Common.Persistence;
using Echoes.Application.Common.Services;
using Echoes.Domain.Repository;
using Echoes.Infrastructure.Identity;
using Echoes.Infrastructure.Persistence;
using Echoes.Infrastructure.Persistence.Repositories;
using Echoes.Infrastructure.Security;
using Echoes.Infrastructure.Services;
using Echoes.Shared.Abstraction.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Echoes.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<GameDbContext>(options => options.UseNpgsql(connectionString));

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);
            services
                .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Secret)
                        ),
                    };
                });
            services.AddAuthorization();
            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<GameDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserNamePolicy, UserNamePolicy>();
            services.AddScoped<IEmailPolicy, EmailPolicy>();
            services.AddScoped<IPasswordPolicy, PasswordPolicy>();
            services.AddScoped<IRegistrationPolicy, RegistrationPolicy>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<DotnetBadWordDetector.ProfanityDetector>();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
