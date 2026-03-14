using Echoes.Shared.Network.Common.Protocol;

namespace Echoes.Application.Network
{
    /// <summary>
    /// Defines a contract for processing specific network packets within the application layer.
    /// </summary>
    /// <typeparam name="T">The specific packet type to handle, must implement <see cref="IPacketUnion"/>.</typeparam>
    /// <remarks>
    /// Implementing this interface allows the systems to cleanly separate business logic from
    /// networking infrastructure. The <see cref="PacketDispatcher"/> will automatically
    /// resolve the appropriate implementation of this interface based on the incoming packet type.
    /// </remarks>
    public interface IPacketHandler<in T>
        where T : IPacketUnion
    {
        /// <summary>
        /// Executes the business logic associated with the received packet.
        /// </summary>
        /// <param name="senderId">The <see cref="Guid"/> of the client that sent the packet.</param>
        /// <param name="packet">The strongly-typed packet payload to process.</param>
        public void Handle(Guid senderId, T packet);
    }
}
