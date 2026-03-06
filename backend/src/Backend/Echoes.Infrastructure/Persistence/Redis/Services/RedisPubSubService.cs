using System.Text.Json;
using Echoes.Application.Common.Persistence;
using Echoes.Infrastructure.Persistence.Redis.Multiplexers;
using StackExchange.Redis;

namespace Echoes.Infrastructure.Persistence.Redis.Services;

public class RedisPubSubService(IPubSubMultiplexer multiplexer) : IPubSubService
{
    private readonly ISubscriber _subscriber = multiplexer.Connection.GetSubscriber();

    public async Task PublishAsync<T>(string channel, T message)
    {
        var json = JsonSerializer.Serialize(message);
        await _subscriber.PublishAsync(RedisChannel.Literal(channel), json);
    }

    public async Task SubscribeAsync<T>(string channel, Action<T> handler)
    {
        await _subscriber.SubscribeAsync(
            RedisChannel.Literal(channel),
            (ch, value) =>
            {
                if (value.IsNullOrEmpty)
                    return;
                var message = JsonSerializer.Deserialize<T>(value!);
                if (message != null)
                    handler(message);
            }
        );
    }
}
