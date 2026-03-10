namespace Echoes.Application.Connection;

/// <summary>
/// Defines the lifecycle stages of a player session.
/// </summary>
/// <remarks>
/// This state machine is critical for security and flow control. It ensures
/// the server only processes packets appropriate to the current phase of the
/// client's connection (e.g., preventing movement commands from unauthenticated peers).
/// </remarks>
public enum SessionState
{
    /// <summary>
    /// The physical socket is established, but the client has not verified their identity.
    /// Only allows 'Handshake' or 'Auth' packets.
    /// </summary>
    Connected,

    /// <summary>
    /// The client's identity and ticket have been verified.
    /// Allows character selection and initial world data synchronization.
    /// </summary>
    Authenticated,

    /// <summary>
    /// The character is fully instantiated and active in the ECS world.
    /// Allows gameplay packets like 'Move' or 'Attack'.
    /// </summary>
    InGame,
}
