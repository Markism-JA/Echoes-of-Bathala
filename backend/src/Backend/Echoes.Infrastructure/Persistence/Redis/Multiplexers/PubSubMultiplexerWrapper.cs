using StackExchange.Redis;

namespace Echoes.Infrastructure.Persistence.Redis.Multiplexers
{
    /// <summary>
    /// A concrete implementation of <see cref="IPubSubMultiplexer"/> that wraps a physical Redis connection.
    /// </summary>
    /// <param name="connection">The underlying Redis connection to be used for Pub/Sub.</param>
    /// <remarks>
    /// Acts as a bridge between the generic <see cref="IConnectionMultiplexer"/> and the
    /// application's domain-specific requirement for a messaging-only connection.
    /// </remarks>
    public class PubSubMultiplexerWrapper(IConnectionMultiplexer connection) : IPubSubMultiplexer
    {
        /// <inheritdoc />
        public IConnectionMultiplexer Connection { get; } = connection;
    }
}
