using System.Text.Json;
using Echoes.Application.Persistence.Abstractions;
using Echoes.Infrastructure.Persistence.Redis.Multiplexers;
using StackExchange.Redis;

namespace Echoes.Infrastructure.Persistence.Redis.Services;

public class RedisBufferService(IBufferMultiplexer multiplexer) : IBufferService
{
    private readonly IDatabase _database = multiplexer.Connection.GetDatabase();

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

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;
        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}
