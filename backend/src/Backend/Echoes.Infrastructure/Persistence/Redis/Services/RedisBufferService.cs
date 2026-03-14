using System.Text.Json;
using Echoes.Application.Persistence.Abstractions;
using Echoes.Infrastructure.Persistence.Redis.Multiplexers;
using StackExchange.Redis;

namespace Echoes.Infrastructure.Persistence.Redis.Services;

/// <summary>
/// A dumb implementation of <see cref="IBufferService"/> for generic,
/// high-frequency key-value operations.
/// </summary>
/// <remarks>
/// <b>Use Case:</b> This service is strictly for generic Read/Write operations
/// (e.g., caching, temporary state).
/// </remarks>
public class RedisBufferService(IBufferMultiplexer multiplexer) : IBufferService
{
    private readonly IDatabase _database = multiplexer.Connection.GetDatabase();

    /// <inheritdoc />
    /// <remarks>
    /// Performs a generic <c>SET</c> operation. Serializes <typeparamref name="T"/> to JSON.
    /// </remarks>
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var json = JsonSerializer.Serialize(value);

        await _database.StringSetAsync(
            key,
            (RedisValue)json,
            expiration,
            When.Always,
            CommandFlags.None
        );
    }

    /// <inheritdoc />
    /// <remarks>
    /// Performs a generic <c>GET</c> operation. Returns default of <typeparamref name="T"/> if the key is missing or invalid.
    /// </remarks>
    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;
        return JsonSerializer.Deserialize<T>(value!);
    }

    /// <summary>
    /// Removes a specific key from the buffer instance.
    /// </summary>
    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}
