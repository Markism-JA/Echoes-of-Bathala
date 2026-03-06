using StackExchange.Redis;

namespace Echoes.Infrastructure.Persistence.Redis.Multiplexers
{
    public interface IPubSubMultiplexer
    {
        public IConnectionMultiplexer Connection { get; }
    }
}
