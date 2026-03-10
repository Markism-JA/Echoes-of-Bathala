namespace Echoes.Application.Connection;

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
    /// The unique identifier for the character currently controlled by this session.
    /// </summary>
    public Guid? ActiveCharacterId { get; set; }

    /// <summary>
    /// The ECS entity ID representing the player in the simulation world.
    /// </summary>
    public int? EntityId { get; set; }

    /// <summary>
    /// The unique account identifier for the authenticated user.
    /// </summary>
    public Guid? AccountId { get; set; }

    /// <summary>
    /// The current lifecycle state of the session.
    /// </summary>
    public SessionState State { get; set; } = SessionState.Connected;

    /// <summary>
    /// The UTC timestamp of the most recent heartbeat packet received.
    /// Used by the server to identify stale/timed-out connections.
    /// </summary>
    public DateTime LastHeartbeat { get; set; } = DateTime.UtcNow;
}
