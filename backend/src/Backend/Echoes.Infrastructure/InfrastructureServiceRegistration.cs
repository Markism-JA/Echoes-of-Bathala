using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Echoes.Application.Common.Identity;
using Echoes.Application.Common.Persistence;
using Echoes.Application.Common.Serialization;
using Echoes.Application.Common.Services;
using Echoes.Domain.Repository;
using Echoes.Infrastructure.Common.Serialization;
using Echoes.Infrastructure.Identity;
using Echoes.Infrastructure.Persistence.Postgresql;
using Echoes.Infrastructure.Persistence.Postgresql.Repositories;
using Echoes.Infrastructure.Persistence.Redis.Multiplexers;
using Echoes.Infrastructure.Persistence.Redis.Services;
using Echoes.Infrastructure.Security;
using Echoes.Infrastructure.Services;
using Echoes.Shared.Abstraction.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

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

    public static IServiceCollection AddPubSubRedisInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("RedisPubSub");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Could not find a 'RedisBuffer' connection string in appsettings.json."
            );
        }
        var connection = ConnectionMultiplexer.Connect(connectionString);
        services.AddKeyedSingleton<IConnectionMultiplexer>("PubSub", connection);
        services.AddSingleton<IPubSubMultiplexer>(new PubSubMultiplexerWrapper(connection));

        services.AddSingleton<IPubSubService, RedisPubSubService>();

        return services;
    }

    public static IServiceCollection AddBufferRedisInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("RedisBuffer");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Could not find a 'RedisBuffer' connection string in appsettings.json."
            );
        }
        var connection = ConnectionMultiplexer.Connect(connectionString);
        services.AddKeyedSingleton<IConnectionMultiplexer>("Buffer", connection);
        services.AddSingleton<IBufferMultiplexer>(new BufferMultiplexerWrapper(connection));

        services.AddSingleton<ISessionService, RedisSessionService>();

        services.AddSingleton<IBufferService, RedisBufferService>();

        return services;
    }

    public static IServiceCollection AddSerializationInfrastructure(
        this IServiceCollection services
    )
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false,

            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            Converters = { new JsonStringEnumConverter() },
        };
        services.TryAddSingleton(jsonOptions);

        services.AddKeyedSingleton<ISerializer, JsonSerializationService>("Json");

        services.AddKeyedSingleton<ISerializer, MessagePackSerializationService>("MessagePack");

        return services;
    }
}
