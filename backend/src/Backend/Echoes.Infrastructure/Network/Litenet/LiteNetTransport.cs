using System.Buffers;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Echoes.Application.Common.Abstractions;
using Echoes.Application.Network;
using Echoes.Infrastructure.Network.Configuration;
using Echoes.Shared.Network.Common.Protocol;
using LiteNetLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Echoes.Infrastructure.Network.Litenet;

/// <summary>
/// A concrete implementation of <see cref="INetworkTransport"/> using LiteNetLib.
/// Acts as an adapter, translating raw UDP socket events into application-level game events.
/// </summary>
/// <remarks>
/// The <see cref="LiteNetTransport"/> serves as the boundary between external network
/// I/O and the internal Application logic. Its primary responsibilities include:
/// <list type="number">
/// <item><description>Mapping physical <see cref="NetPeer"/> instances to stable <see cref="Guid"/> identifiers.</description></item>
/// <item><description>Protecting the Game Simulation from malformed packet data through robust error boundaries.</description></item>
/// </list>
/// This implementation is optimized for the high-concurrency requirements of an MMORPG backend.
/// </remarks>
public class LiteNetTransport : INetworkTransport, INetEventListener
{
    private readonly NetManager _netManager;
    private readonly ISerializer _serializer;
    private readonly PacketDispatcher _packetDispatcher;
    private readonly ILogger<LiteNetTransport> _logger;
    private readonly NetworkOptions _options;

    // NOTE: Concurrent dictionaries is good enoug for now. Optional: If traffic becomes massive, consider lock-free queues for sending or batching network writes to reduce contention.
    private readonly ConcurrentDictionary<Guid, int> _packetCount = new();

    // TODO: Implement per-second reset using System.Timers.Timer or Stopwatch to enforce _maxPacketsPerSecond.
    // Possible implementatin may include a sliding window counter for smoother enforcement.

    // TODO: Validate incoming packets fully.
    // Ensure IPacketUnion deserialization doesn’t throw on malformed bytes.
    // Reject or log invalid packet types.
    // Optional: Implement anti-spam / anti-DOS measures (e.g., dropping clients sending >N invalid packets).

    // Currently: Peer disconnects just remove mappings.
    // TODO: Send explicit disconnection packets if needed by client.
    // Optional: Track “graceful disconnect” vs “timeout” and mark entity session as Disconnected for 30–60s for quick reconnects (already noted in TODO).

    //Currently: Logs latency >250ms.
    // TODO: Integrate with simulation lag compensation:
    // RTT-aware interpolation for players.
    // Optionally, integrate with client prediction / reconciliation system.

    // Currently: Critical exceptions in OnNetworkReceive may be caught but you only log.
    // TODO: Consider: Metrics for malformed packet frequency.
    // Optional disconnect / quarantine for malicious clients.

    /// <summary>
    /// Maps LiteNetLib's internal integer IDs to the Application's global Guid IDs.
    /// </summary>
    private readonly ConcurrentDictionary<int, Guid> _peerIdToConnectionId = new();

    /// <summary>
    /// Maps the Application's global Guid IDs back to LiteNetLib's physical network peers.
    /// Required for routing outbound packets to the correct socket.
    /// </summary>
    private readonly ConcurrentDictionary<Guid, NetPeer> _connectionIdToPeer = new();

    /// <inheritdoc />
    public event Action<Guid>? OnPlayerConnected;

    /// <inheritdoc />
    public event Action<Guid>? OnPlayerDisconnected;

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteNetTransport"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="packetDispatcher">The router responsible for forwarding deserialized packets to game systems.</param>
    /// <param name="serializer">The keyed serialization service (e.g., MemoryPack) used to decode payloads.</param>
    /// <param name="options">The network configuration options, such as the port to bind to.</param>
    public LiteNetTransport(
        ILogger<LiteNetTransport> logger,
        PacketDispatcher packetDispatcher,
        [FromKeyedServices("MemoryPack")] ISerializer serializer,
        IOptions<NetworkOptions> options,
        INetworkEngineFactory factory
    )
    {
        _logger = logger;
        _packetDispatcher = packetDispatcher;
        _serializer = serializer;
        _netManager = factory.Create(this);
        _options = options.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled(cancellationToken);
        }

        Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled(cancellationToken);
        }

        Stop();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public void Start()
    {
        if (!_netManager.Start(_options.Port))
        {
            _logger.LogCritical(
                "Failed to start LiteNetLib on port {Port}. Is the port already in use?",
                _options.Port
            );
            throw new InvalidOperationException(
                $"Transport failed to bind to port {_options.Port}"
            );
        }

        _logger.LogInformation("LiteNetLib Transport started on port {Port}.", _options.Port);
    }

    /// <inheritdoc />
    public void Stop() => _netManager.Stop();

    /// <inheritdoc />
    public void PollEvents() => _netManager.PollEvents();

    // TODO: Support multiple channels (reliable/unreliable, ordered/unordered) instead of only ReliableOrdered.
    // Optional: Implement packet compression or delta snapshot logic integrated with MemoryPack.

    /// <inheritdoc />
    public void Send(Guid connectionId, ReadOnlySpan<byte> data)
    {
        if (_connectionIdToPeer.TryGetValue(connectionId, out var peer))
        {
            peer.Send(data, DeliveryMethod.ReliableOrdered);
        }
    }

    // Currently: Broadcast sends to all peers.
    // TODO: Implement AOI-aware broadcast for scalability.
    // Later integrate with dirty AOI + snapshot delta compression.

    /// <inheritdoc />
    /// <remarks>
    /// Sends data to all connected peers. utilizes <see cref="ArrayPool{T}"/>
    /// to avoid heap allocations during the broadcast operation.
    /// </remarks>
    public void Broadcast(ReadOnlySpan<byte> data)
    {
        byte[] buffer = ArrayPool<byte>.Shared.Rent(data.Length);

        try
        {
            data.CopyTo(buffer);

            _netManager.SendToAll(buffer, 0, data.Length, DeliveryMethod.ReliableOrdered);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    /// <inheritdoc />
    public void Disconnect(Guid connectionId)
    {
        if (_connectionIdToPeer.TryGetValue(connectionId, out var peer))
        {
            //TODO: Implementaton packet for disconnection code.
            peer.Disconnect();
        }
    }

    /// <summary>
    /// Handles incoming connection requests before a full connection is established.
    /// </summary>
    public void OnConnectionRequest(ConnectionRequest request)
    {
        if (_netManager.ConnectedPeersCount < _options.MaxConnections)
        {
            request.AcceptIfKey(_options.ConnectionKey);
        }
        else
        {
            request.Reject();
            _logger.LogWarning("Connection rejected: Server is full.");
        }
    }

    /// <summary>
    /// Triggered when a physical network peer successfully connects.
    /// Bridges the connection by generating a Guid and mapping it.
    /// </summary>
    public void OnPeerConnected(NetPeer peer)
    {
        var connectionId = Guid.NewGuid();

        _peerIdToConnectionId[peer.Id] = connectionId;
        _connectionIdToPeer[connectionId] = peer;

        var remoteEndpoint = $"{peer.Address}:{peer.Port}";
        _logger.LogInformation(
            "Peer {PeerId} connected from {RemoteEndpoint}. Assigned ConnectionId: {ConnectionId}",
            peer.Id,
            remoteEndpoint,
            connectionId
        );

        OnPlayerConnected?.Invoke(connectionId);
    }

    /// <summary>
    /// Triggered when a peer disconnects (gracefully or via timeout).
    /// Cleans up internal routing maps and notifies the application layer.
    /// </summary>
    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        // NOTE: DisconnectInfo can also contain additional data, it's useful in analytics
        // to get the actual reason for the disconnection.

        // TODO: Handling disconnection.
        // Don't immediately delete player data from game world, Mark player-session as
        // disconnected but keep their character for a certain time.
        // e.g. 30-60 seconds. If they reconnected, rebind them to the session in the memory.
        if (_peerIdToConnectionId.TryRemove(peer.Id, out var connectionId))
        {
            _connectionIdToPeer.TryRemove(connectionId, out _);

            _logger.LogInformation(
                "Peer disconnected. ConnectionId: {ConnectionId}. Reason: {Reason}. Error: {SocketError}",
                connectionId,
                disconnectInfo.Reason,
                disconnectInfo.SocketErrorCode
            );

            OnPlayerDisconnected?.Invoke(connectionId);
        }
    }

    /// <summary>
    /// Triggered when a packet arrives from a connected peer.
    /// </summary>
    public void OnNetworkReceive(
        NetPeer peer,
        NetPacketReader reader,
        byte channelNumber,
        DeliveryMethod deliveryMethod
    )
    {
        if (!_peerIdToConnectionId.TryGetValue(peer.Id, out var connectionId))
        {
            reader.Recycle();
            return;
        }

        try
        {
            if (reader.UserDataSize > _options.MaxPacketPerSize)
            {
                _logger.LogWarning(
                    "Oversized packet from {Id}: {Size} bytes. Disconnecting.",
                    connectionId,
                    reader.UserDataSize
                );
                Disconnect(connectionId);
                return;
            }

            // TODO: Employ rate limitting. In a real app, reset this count every 1 second
            // via a timer or stopwatch.
            var count = _packetCount.AddOrUpdate(connectionId, 1, (_, val) => val + 1);
            if (count > _options.MaxPacketPerSecond)
            {
                return;
            }

            ReadOnlyMemory<byte> data = new(
                reader.RawData,
                reader.UserDataOffset,
                reader.UserDataSize
            );
            var packet = _serializer.Deserialize<IPacketUnion>(data);

            if (packet != null)
            {
                _packetDispatcher.Dispatch(connectionId, packet);
            }
        }
        finally
        {
            reader.Recycle();
        }
    }

    /// <summary>
    /// Handles low-level socket errors.
    /// </summary>
    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        _logger.LogError("Network error at {EndPoint}: {ErrorCode}", endPoint, socketError);
    }

    /// <summary>
    /// Triggered when receiving a packet from a peer that has not established a connection.
    /// </summary>
    public void OnNetworkReceiveUnconnected(
        IPEndPoint remoteEndPoint,
        NetPacketReader reader,
        UnconnectedMessageType messageType
    )
    {
        // NOTE: Typically used for LAN discovery (pinging for server list). Ignored. Possibly useful in dev?
    }

    /// <summary>
    /// Periodic update regarding the round-trip latency of a peer.
    /// </summary>
    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        if (_peerIdToConnectionId.TryGetValue(peer.Id, out var connectionId))
        {
            // _sessionManager.UpdateLatency(connectionId, latency);
            // TODO: Pass this latency value to the Session/Application layer
            // to drive the Lag Compensation system.

            // TODO: backward reconciliation. This is System Specific.
            if (latency > 250)
            {
                _logger.LogWarning(
                    "Latency Spike: Connection {Id} is at {Ping}ms",
                    connectionId,
                    latency
                );
            }
        }
    }
}
