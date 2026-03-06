using System.Text;
using Echoes.Application.Common.Identity;
using Echoes.Application.Common.Persistence;
using Echoes.Application.Common.Services;
using Echoes.Domain.Repository;
using Echoes.Infrastructure.Identity;
using Echoes.Infrastructure.Persistence.Postgresql;
using Echoes.Infrastructure.Persistence.Postgresql.Repositories;
using Echoes.Infrastructure.Security;
using Echoes.Infrastructure.Services;
using Echoes.Shared.Abstraction.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Echoes.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabaseInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<GameDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }

    public static IServiceCollection AddAuthInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();

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

        return services;
    }

    public static IServiceCollection AddRepositoryInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }

    public static IServiceCollection AddPolicyAndUtilityServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<DotnetBadWordDetector.ProfanityDetector>();

        services.AddScoped<IUserNamePolicy, UserNamePolicy>();
        services.AddScoped<IEmailPolicy, EmailPolicy>();
        services.AddScoped<IPasswordPolicy, PasswordPolicy>();
        services.AddScoped<IRegistrationPolicy, RegistrationPolicy>();
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
