using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Echoes.Application.Core.Abstractions;
using Echoes.Application.Network;
using Echoes.Infrastructure.Networking.Configuration;
using Echoes.Shared.Network.Common.Protocol;
using LiteNetLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Echoes.Infrastructure.Networking;

/// <summary>
/// A concrete implementation of <see cref="INetworkTransport"/> using LiteNetLib.
/// Acts as an adapter, translating raw UDP socket events into application-level game events.
/// </summary>
/// <remarks>
/// This class isolates the application layer from LiteNetLib's specific types (like <see cref="NetPeer"/>),
/// ensuring the game logic only ever interacts with generic <see cref="Guid"/> connection identifiers.
/// </remarks>
public class LiteNetTransport : INetworkTransport, INetEventListener
{
    private readonly NetManager _netManager;
    private readonly ISerializer _serializer;
    private readonly PacketDispatcher _packetDispatcher;
    private readonly ILogger<LiteNetTransport> _logger;
    private readonly NetworkOptions _options;

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
        Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Stop();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public void Start()
    {
        _netManager.Start(_options.Port);
        _logger.LogInformation("LiteNetLib Transport started on port {Port}.", _options.Port);
    }

    /// <inheritdoc />
    public void Stop() => _netManager.Stop();

    /// <inheritdoc />
    public void PollEvents() => _netManager.PollEvents();

    /// <inheritdoc />
    public void Send(Guid connectionId, ReadOnlySpan<byte> data)
    {
        if (_connectionIdToPeer.TryGetValue(connectionId, out var peer))
        {
            // LiteNetLib requires a byte array.
            // If data is backed by an array, ToArray() creates a copy.
            // For extreme optimization later, an option is to use NetDataWriter or PooledPacket.
            peer.Send(data.ToArray(), DeliveryMethod.ReliableOrdered);
        }
        else
        {
            _logger.LogWarning(
                "Attempted to send data to unknown connection: {ConnectionId}",
                connectionId
            );
        }
    }

    /// <inheritdoc />
    public void Broadcast(ReadOnlySpan<byte> data)
    {
        _netManager.SendToAll(data.ToArray(), DeliveryMethod.ReliableOrdered);
    }

    /// <inheritdoc />
    public void Disconnect(Guid connectionId)
    {
        if (_connectionIdToPeer.TryGetValue(connectionId, out var peer))
        {
            peer.Disconnect();
        }
    }

    /// <summary>
    /// Handles incoming connection requests before a full connection is established.
    /// </summary>
    public void OnConnectionRequest(ConnectionRequest request)
    {
        // Accept the incoming connection.
        // Note: You can add connection key validation here later (e.g. request.AcceptIfKey("MyGameV1"))
        request.Accept();
    }

    /// <summary>
    /// Triggered when a physical network peer successfully connects.
    /// Bridges the connection by generating a Guid and mapping it.
    /// </summary>
    public void OnPeerConnected(NetPeer peer)
    {
        var connectionId = Guid.NewGuid();

        // Establish the two-way mapping
        _peerIdToConnectionId[peer.Id] = connectionId;
        _connectionIdToPeer[connectionId] = peer;

        var remoteEndpoint = $"{peer.Address}:{peer.Port}";
        _logger.LogInformation(
            "Peer {PeerId} connected from {RemoteEndpoint}. Assigned ConnectionId: {ConnectionId}",
            peer.Id,
            remoteEndpoint,
            connectionId
        );

        // Notify the application layer
        OnPlayerConnected?.Invoke(connectionId);
    }

    /// <summary>
    /// Triggered when a peer disconnects (gracefully or via timeout).
    /// Cleans up internal routing maps and notifies the application layer.
    /// </summary>
    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        if (_peerIdToConnectionId.TryRemove(peer.Id, out var connectionId))
        {
            _connectionIdToPeer.TryRemove(connectionId, out _);

            _logger.LogInformation(
                "Peer disconnected. ConnectionId: {ConnectionId}. Reason: {Reason}",
                connectionId,
                disconnectInfo.Reason
            );

            // Notify the application layer
            OnPlayerDisconnected?.Invoke(connectionId);
        }
    }

    /// <summary>
    /// Triggered when a packet arrives from a connected peer.
    /// </summary>
    /// <remarks>
    /// This method is in the "hot path" and uses <see cref="ReadOnlyMemory{T}"/>
    /// to avoid unnecessary heap allocations during deserialization.
    /// </remarks>
    public void OnNetworkReceive(
        NetPeer peer,
        NetPacketReader reader,
        byte channelNumber,
        DeliveryMethod deliveryMethod
    )
    {
        if (!_peerIdToConnectionId.TryGetValue(peer.Id, out var connectionId))
            return;

        try
        {
            // Wrap the buffer in ReadOnlyMemory to satisfy the generic serialization contract safely
            var data = new ReadOnlyMemory<byte>(reader.GetRemainingBytes());
            var packet = _serializer.Deserialize<IPacketUnion>(data);

            if (packet != null)
            {
                // Decoupled execution: Hand off the packet to the application's routing logic
                _packetDispatcher.Dispatch(connectionId, packet);
            }
        }
        catch (Exception ex)
        {
            // Catching exceptions here is critical; a bad packet from a malicious client
            // should not crash the server's main network polling thread.
            _logger.LogError(
                ex,
                "Failed to deserialize packet from ConnectionId {ConnectionId}",
                connectionId
            );
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
        // Typically used for server discovery (pinging for server list). Ignored for now.
    }

    /// <summary>
    /// Periodic update regarding the round-trip latency of a peer.
    /// </summary>
    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        // Useful for lag compensation, but can be safely ignored for basic connectivity.
    }
}
