using StackExchange.Redis;

namespace Echoes.Infrastructure.Persistence.Redis.Multiplexers
{
    /// <summary>
    /// Represents a specialized Redis connection multiplexer dedicated to Publisher/Subscriber (Pub/Sub) messaging.
    /// </summary>
    /// <remarks>
    /// Pub/Sub operations in Redis are often long-lived and can benefit from a separate connection
    /// to avoid head-of-line blocking or interference with standard database commands (GET/SET).
    /// This interface allows the DI container to provide the specific connection meant for event broadcasting.
    /// </remarks>
    public interface IPubSubMultiplexer
    {
        /// <summary>
        /// Gets the underlying <see cref="IConnectionMultiplexer"/> dedicated to messaging.
        /// </summary>
        /// <value>
        /// The active connection used for subscribing to channels and publishing game events.
        /// </value>
        public IConnectionMultiplexer Connection { get; }
    }
}
