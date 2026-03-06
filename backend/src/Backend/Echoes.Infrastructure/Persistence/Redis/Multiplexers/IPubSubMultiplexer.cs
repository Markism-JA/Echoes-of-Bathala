using StackExchange.Redis;

namespace Echoes.Infrastructure.Persistence.Redis.Multiplexers
{
    public interface IBufferMultiplexer
    {
        public IConnectionMultiplexer Connection { get; }
    }
}
