using StackExchange.Redis;

namespace Echoes.Infrastructure.Persistence.Redis.Multiplexers
{
    /// <summary>
    /// Represents a specialized Redis connection multiplexer dedicated to buffering operations.
    /// </summary>
    /// <remarks>
    /// This interface acts as a "marker" to distinguish the high-frequency buffer connection
    /// from other Redis connections in the application. It follows the <b>Interface Segregation</b>
    /// and <b>Dependency Inversion</b> principles by allowing services to depend on a
    /// specific purpose-driven connection rather than a generic <see cref="IConnectionMultiplexer"/>.
    /// </remarks>
    public interface IBufferMultiplexer
    {
        /// <summary>
        /// Gets the underlying <see cref="IConnectionMultiplexer"/> for this buffer.
        /// </summary>
        /// <value>
        /// The active Redis connection used for temporary data, queues, or high-throughput buffering.
        /// </value>
        public IConnectionMultiplexer Connection { get; }
    }
}
