using System.Collections.Concurrent;
using Echoes.Application.Network;

namespace Echoes.Application.Session;

/// <summary>
/// Manages the lifecycle of active network sessions.
/// Acts as the single source of truth for which connections are currently active on the server.
/// </summary>
/// <remarks>
/// This service consumes events from <see cref="INetworkTransport"/> to maintain
/// a thread-safe registry of connected players.
/// </remarks>
public class SessionManager : IDisposable
{
    private readonly INetworkTransport _transport;
    private readonly ConcurrentDictionary<Guid, PlayerSession> _sessions = new();

    /// <summary>
    /// Invoked when a new session has been initialized and registered.
    /// </summary>
    public event Action<PlayerSession>? PlayerJoined;

    /// <summary>
    /// Invoked when a session has been removed from the session registry.
    /// </summary>
    public event Action<Guid>? PlayerLeft;

    /// <summary>
    /// Initializes the manager and hooks into the transport layer events.
    /// </summary>
    /// <param name="transport">The underlying network provider.</param>
    public SessionManager(INetworkTransport transport)
    {
        _transport = transport;
        transport.OnPlayerConnected += HandlePlayerConnected;
        transport.OnPlayerDisconnected += HandlePlayerDisconnected;
    }

    private void HandlePlayerConnected(Guid connectionId)
    {
        var session = new PlayerSession(connectionId, DateTime.UtcNow);

        // ConcurrentDictionary ensures thread-safety if multiple transport
        // threads attempt to register sessions simultaneously.
        if (_sessions.TryAdd(connectionId, session))
        {
            PlayerJoined?.Invoke(session);
        }
    }

    private void HandlePlayerDisconnected(Guid connectionId)
    {
        if (_sessions.TryRemove(connectionId, out _))
        {
            PlayerLeft?.Invoke(connectionId);
        }
    }

    /// <summary>
    /// Checks if a connection currently has an active session.
    /// </summary>
    public bool IsConnected(Guid connectionId) => _sessions.ContainsKey(connectionId);

    /// <summary>
    /// Returns a snapshot of all currently active sessions.
    /// </summary>
    public IReadOnlyCollection<PlayerSession> GetAllSessions() =>
        (IReadOnlyCollection<PlayerSession>)_sessions.Values;

    /// <summary>
    /// Retrieves a session by its connection ID.
    /// </summary>
    /// <returns>The session if found; otherwise, null.</returns>
    public PlayerSession? GetSession(Guid connectionId)
    {
        _sessions.TryGetValue(connectionId, out var session);
        return session;
    }

    /// <summary>
    /// Releases the resources used by the <see cref="SessionManager"/>.
    /// </summary>
    /// <remarks>
    /// This is crucial for preventing memory leaks and duplicate event execution.
    /// Unsubscribing from the <see cref="INetworkTransport"/> events ensures that
    /// this instance can be garbage collected and won't respond to network events
    /// if a new manager is instantiated.
    /// </remarks>
    public void Dispose()
    {
        _transport.OnPlayerConnected -= HandlePlayerConnected;
        _transport.OnPlayerDisconnected -= HandlePlayerDisconnected;
        _sessions.Clear();
    }
}
