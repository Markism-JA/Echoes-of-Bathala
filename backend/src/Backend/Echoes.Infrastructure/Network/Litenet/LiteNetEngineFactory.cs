using Echoes.Infrastructure.Network;
using LiteNetLib;

namespace Echoes.Infrastructure.Networking
{
    /// <summary>
    /// A factory responsible for the instantiation of the LiteNetLib <see cref="NetManager"/>.
    /// </summary>
    /// <remarks>
    /// This implementation follows the Factory Pattern to decouple the <see cref="LiteNetTransport"/>
    /// from the specific configuration and lifecycle management of the underlying network engine.
    /// </remarks>
    public class LiteNetEngineFactory : INetworkEngineFactory
    {
        /// <summary>
        /// Creates a new <see cref="NetManager"/> instance configured with the specified <paramref name="listener"/>.
        /// </summary>
        /// <param name="listener">
        /// The event listener (typically an instance of <see cref="LiteNetTransport"/>)
        /// that will receive and handle low-level network events like peer connections and packet reception.
        /// </param>
        /// <returns>
        /// A configured <see cref="NetManager"/> ready to be started.
        /// </returns>
        public NetManager Create(INetEventListener listener)
        {
            return new NetManager(listener);
        }
    }
}
