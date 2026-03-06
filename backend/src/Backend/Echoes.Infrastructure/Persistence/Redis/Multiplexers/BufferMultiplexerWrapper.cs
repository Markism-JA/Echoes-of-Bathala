using StackExchange.Redis;

namespace Echoes.Infrastructure.Persistence.Redis.Multiplexers
{
    public class BufferMultiplexerWrapper(IConnectionMultiplexer connection) : IBufferMultiplexer
    {
        public IConnectionMultiplexer Connection { get; } = connection;
    }
}
