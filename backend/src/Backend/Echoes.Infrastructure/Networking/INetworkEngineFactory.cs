using LiteNetLib;

namespace Echoes.Infrastructure.Networking
{
    /// <summary>
    /// Defines the contract for a factory responsible for instantiating network engines.
    /// </summary>
    /// <remarks>
    /// By abstracting engine creation, this interface allows for easier dependency
    /// injection and unit testing, as the concrete networking library (e.g., LiteNetLib)
    /// does not need to be instantiated directly by the transport layer.
    /// </remarks>
    public interface INetworkEngineFactory
    {
        /// <summary>
        /// Creates a new instance of the underlying network engine.
        /// </summary>
        /// <param name="listener">
        /// The event listener instance that will receive lifecycle and packet events from the engine.
        /// </param>
        /// <returns>
        /// A configured <see cref="NetManager"/> instance ready for server startup.
        /// </returns>
        public NetManager Create(INetEventListener listener);
    }
}
