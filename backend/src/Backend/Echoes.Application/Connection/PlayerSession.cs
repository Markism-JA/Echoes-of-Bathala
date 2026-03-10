namespace Echoes.Application.Connection
{
    /// <summary>
    /// Represents an immutable snapshot of an active player session.
    /// </summary>
    /// <remarks>
    /// This record is used by the Session Manager to track active connections,
    /// enforce timeouts, and correlate network peers with game world entities.
    /// </remarks>
    public record PlayerSession(Guid ConnectionId, DateTime ConnectedAt)
    {
        /// <summary>
        /// The unique identifier assigned by the transport layer to the network peer.
        /// </summary>
        public Guid ConnectionId = ConnectionId;

        /// <summary>
        /// The UTC timestamp when the session was initialized.
        /// </summary>
        public DateTime ConnectedAt = ConnectedAt;
    }
}
