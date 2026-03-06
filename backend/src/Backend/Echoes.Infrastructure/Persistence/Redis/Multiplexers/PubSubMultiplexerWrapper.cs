using StackExchange.Redis;

namespace Echoes.Infrastructure.Persistence.Redis.Multiplexers
{
    public class PubSubMultiplexerWrapper(IConnectionMultiplexer connection) : IPubSubMultiplexer
    {
        public IConnectionMultiplexer Connection { get; } = connection;
    }
}
