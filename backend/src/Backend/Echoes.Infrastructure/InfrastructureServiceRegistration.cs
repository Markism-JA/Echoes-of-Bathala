using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Echoes.Application.Auth.Abstractions;
using Echoes.Application.Auth.Models;
using Echoes.Application.Auth.Policies;
using Echoes.Application.Core.Abstractions;
using Echoes.Application.Core.Services;
using Echoes.Application.Network;
using Echoes.Application.Persistence.Abstractions;
using Echoes.Domain.Common;
using Echoes.Domain.Users.Persistence;
using Echoes.Infrastructure.Auth.Policies;
using Echoes.Infrastructure.Identity;
using Echoes.Infrastructure.Networking;
using Echoes.Infrastructure.Networking.Configuration;
using Echoes.Infrastructure.Persistence.Postgresql;
using Echoes.Infrastructure.Persistence.Postgresql.Repositories;
using Echoes.Infrastructure.Persistence.Redis.Multiplexers;
using Echoes.Infrastructure.Persistence.Redis.Services;
using Echoes.Infrastructure.Serialization;
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
            MaxDepth = 128,
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

        services.AddKeyedSingleton<ISerializer, MemoryPackSerializationService>("MemoryPack");

        return services;
    }

    public static IServiceCollection AddNetworkingInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<NetworkOptions>(configuration.GetSection("Network"));
        services.AddSingleton<INetworkEngineFactory, LiteNetEngineFactory>();
        services.AddSingleton<LiteNetTransport>();
        services.AddSingleton<INetworkTransport>(sp => sp.GetRequiredService<LiteNetTransport>());

        return services;
    }
}
