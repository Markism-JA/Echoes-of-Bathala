using StackExchange.Redis;

namespace Echoes.Infrastructure.Persistence.Redis.Multiplexers
{
    /// <summary>
    /// A specialized wrapper for the <see cref="IConnectionMultiplexer"/> dedicated to buffer-related operations.
    /// </summary>
    /// <remarks>
    /// This wrapper allows the Dependency Injection system to differentiate between multiple Redis connections
    /// (e.g., a primary database vs. a high-frequency buffer/cache) by using a specific type
    /// instead of the generic <see cref="IConnectionMultiplexer"/>.
    /// </remarks>
    public class BufferMultiplexerWrapper(IConnectionMultiplexer connection) : IBufferMultiplexer
    {
        /// <summary>
        /// Gets the underlying StackExchange.Redis connection multiplexer.
        /// </summary>
        /// <value>
        /// The active connection used to communicate with the Redis server.
        /// </value>
        public IConnectionMultiplexer Connection { get; } = connection;
    }
}
