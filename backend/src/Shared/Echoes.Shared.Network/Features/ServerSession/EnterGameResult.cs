namespace Echoes.Shared.Network.Features.ServerSession
{
    /// <summary>
    /// Specifies the possible outcomes resulting from an <see cref="ServerSession.EnterGamePacket"/> request.
    /// </summary>
    /// <remarks>
    /// This enum uses a <see cref="byte"/> underlying type to reduce packet size,
    /// ensuring minimal bandwidth overhead during authentication.
    /// </remarks>
    public enum EnterGameResult : byte
    {
        /// <summary>Authentication successful; entry to the game world is granted.</summary>
        Success = 0,

        /// <summary>The provided ticket is malformed, unrecognized, or tampered with.</summary>
        InvalidTicket = 1,

        /// <summary>The ticket is authentic but has passed its expiration window.</summary>
        SessionExpired = 2,

        /// <summary>The account associated with the ticket has been restricted or banned.</summary>
        Banned = 3,

        /// <summary>The server is currently at maximum capacity and cannot accept new connections.</summary>
        ServerFull = 4,

        /// <summary>The server is currently offline for scheduled maintenance.</summary>
        Maintenance = 5,

        /// <summary>An unexpected server-side error occurred during the authentication process.</summary>
        InternalError = 6,
    }
}
