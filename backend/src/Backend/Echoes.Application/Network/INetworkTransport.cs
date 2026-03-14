namespace Echoes.Application.Network;

/// <summary>
/// Defines the abstract contract for the game's network transport layer.
/// This interface isolates game logic from specific networking protocols or libraries.
/// </summary>
public interface INetworkTransport
{
    /// <summary>
    /// Occurs when a new peer has established a connection.
    /// </summary>
    public event Action<Guid> OnPlayerConnected;

    /// <summary>
    /// Occurs when a peer has disconnected from the server.
    /// </summary>
    public event Action<Guid> OnPlayerDisconnected;

    /// <summary>
    /// Initializes the transport layer and starts listening on the specified port.
    /// </summary>
    public void Start();

    /// <summary>
    /// Gracefully shuts down the transport layer and closes all active connections.
    /// </summary>
    public void Stop();

    /// <summary>
    /// Processes incoming network events and packets.
    /// Must be invoked every tick/frame to drive the network simulation.
    /// </summary>
    public void PollEvents();

    /// <summary>
    /// Sends a payload to a specific connected player.
    /// </summary>
    /// <param name="connectionId">The unique ID of the target player.</param>
    /// <param name="data">The serialized byte array to send.</param>
    public void Send(Guid connectionId, ReadOnlySpan<byte> data);

    /// <summary>
    /// Broadcasts a payload to all connected clients.
    /// </summary>
    /// <param name="data">The serialized byte array to send.</param>
    public void Broadcast(ReadOnlySpan<byte> data);

    /// <summary>
    /// Forcefully terminates a connection with a specific peer.
    /// </summary>
    /// <param name="connectionId">The ID of the peer to disconnect.</param>
    public void Disconnect(Guid connectionId);
}
