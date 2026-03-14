using Echoes.Application.Auth.Models;
using Echoes.Application.Common.Abstractions;
using Echoes.Application.Persistence.Abstractions;
using Echoes.Domain.Auth;
using Echoes.Infrastructure.Persistence.Redis.Dto;
using Echoes.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Echoes.Infrastructure.Persistence.Redis.Services;

/// <summary>
/// A specialized Redis-backed service for managing user authentication sessions.
/// </summary>
/// <remarks>
/// Unlike a generic buffer, this service manages relational data across multiple Redis structures:
/// <list type="bullet">
/// <item>
/// <term>Session Key (<c>session:token:{hash}</c>)</term>
/// <description>A Redis String storing serialized session metadata (TTL linked to token expiry).</description>
/// </item>
/// <item>
/// <term>User Index (<c>user:sessions:{userId}</c>)</term>
/// <description>A Redis Sorted Set (ZSET) tracking all active session hashes for a user to enforce session limits.</description>
/// </item>
/// </list>
/// </remarks>
public class RedisSessionService(
    [FromKeyedServices("Buffer")] IConnectionMultiplexer redis,
    IOptions<JwtSettings> jwtOptions,
    [FromKeyedServices("Json")] ISerializer serializer
) : ISessionService
{
    private readonly IDatabase _db = redis.GetDatabase(1);
    private readonly int _maxSessions = jwtOptions.Value.MaxCurrentSessions;

    /// <summary>
    /// Creates a new session and enforces the maximum concurrent session limit per user.
    /// </summary>
    /// <param name="token">The refresh token domain model containing User ID and Expiry.</param>
    /// <remarks>
    /// <b>Process:</b>
    /// <list type="number">
    /// <item><description>Hashes the raw token for secure storage.</description></item>
    /// <item><description>Checks the user's current session count in the Sorted Set.</description></item>
    /// <item><description>If the limit is exceeded, it pops the oldest session (lowest score/expiry) and deletes its metadata.</description></item>
    /// <item><description>Stores new session metadata and adds the hash to the user's index with the expiry as the score.</description></item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Explicitly revokes a session by removing both the metadata and the user index entry.
    /// </summary>
    /// <param name="token">The token to be revoked.</param>
    public async Task RevokeSessionAsync(RefreshToken token)
    {
        var tokenHash = TokenHasher.ComputeHash(token.Token);
        await _db.KeyDeleteAsync($"session:token:{tokenHash}");
        await _db.SortedSetRemoveAsync($"user:sessions:{token.UserId}", tokenHash);
    }

    /// <summary>
    /// Validates if a session exists in Redis.
    /// </summary>
    /// <param name="token">The token to verify.</param>
    /// <returns><c>true</c> if the session hash exists; otherwise <c>false</c>.</returns>
    public async Task<bool> IsSessionValidAsync(RefreshToken token)
    {
        return await _db.KeyExistsAsync($"session:token:{TokenHasher.ComputeHash(token.Token)}");
    }
}
