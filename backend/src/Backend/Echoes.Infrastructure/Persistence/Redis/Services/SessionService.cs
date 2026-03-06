using Echoes.Application.Common.Identity;
using Echoes.Application.Common.Persistence;
using Echoes.Application.Common.Serialization;
using Echoes.Domain;
using Echoes.Infrastructure.Common.Security;
using Echoes.Infrastructure.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Echoes.Infrastructure.Persistence.Redis.Services;

public class RedisSessionService(
    [FromKeyedServices("Buffer")] IConnectionMultiplexer redis,
    IOptions<JwtSettings> jwtOptions,
    [FromKeyedServices("Json")] ISerializer serializer
) : ISessionService
{
    private readonly IDatabase _db = redis.GetDatabase(1);
    private readonly int _maxSessions = jwtOptions.Value.MaxCurrentSessions;

    public async Task CreateSessionAsync(RefreshToken token)
    {
        var tokenHash = TokenHasher.ComputeHash(token.Token);
        var userSessionsKey = $"user:sessions:{token.UserId}";
        var sessionKey = $"session:token:{tokenHash}";

        if (await _db.SortedSetLengthAsync(userSessionsKey) >= _maxSessions)
        {
            var oldest = await _db.SortedSetPopAsync(userSessionsKey, order: Order.Ascending);
            if (oldest.HasValue)
            {
                await _db.KeyDeleteAsync($"session:token:{oldest.Value.Element}");
            }
        }

        var dto = SessionMapper.ToDto(token);
        var value = serializer.Serialize(dto);
        var timeToLive = token.ExpiryDate - DateTime.UtcNow;

        await _db.StringSetAsync(sessionKey, value, timeToLive);
        await _db.SortedSetAddAsync(
            userSessionsKey,
            tokenHash,
            ((DateTimeOffset)token.ExpiryDate).ToUnixTimeSeconds()
        );
    }

    public async Task RevokeSessionAsync(RefreshToken token)
    {
        var tokenHash = TokenHasher.ComputeHash(token.Token);
        await _db.KeyDeleteAsync($"session:token:{tokenHash}");
        await _db.SortedSetRemoveAsync($"user:sessions:{token.UserId}", tokenHash);
    }

    public async Task<bool> IsSessionValidAsync(RefreshToken token)
    {
        return await _db.KeyExistsAsync($"session:token:{TokenHasher.ComputeHash(token.Token)}");
    }
}
