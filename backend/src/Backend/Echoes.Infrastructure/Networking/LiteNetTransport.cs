using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Echoes.Application.Combat;
using Echoes.Application.Connection;
using Echoes.Application.Core.Abstractions;
using Echoes.Application.Core.Protocols;
using Echoes.Application.GameSimulation;
using Echoes.Application.Movement;
using Echoes.Application.Network;
using Echoes.Domain.Combat;
using Echoes.Domain.Networking;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Echoes.Infrastructure.Networking
{
    public class LiteNetTransport : INetworkTransport, INetEventListener
    {
        private readonly ISerializer _serializer;
        private readonly NetManager _netManager;
        private readonly IInputQueue _inputQueue;
        private readonly ILogger<LiteNetTransport> _logger;

        private readonly ConcurrentDictionary<int, Guid> _activeConnections = new();

        public LiteNetTransport(
            IInputQueue inputQueue,
            ILogger<LiteNetTransport> logger,
            [FromKeyedServices("MemoryPack")] ISerializer serializer
        )
        {
            _inputQueue = inputQueue;
            _logger = logger;
            _netManager = new NetManager(this);
            _serializer = serializer;
        }

        public void Start(int port)
        {
            _netManager.Start(port);
            _logger.LogInformation("LiteNetLib Transport started on port {Port}.", port);
        }

        public void Stop() => _netManager.Stop();

        public void PollEvents() => _netManager.PollEvents();

        public void Broadcast(byte[] data)
        {
            _netManager.SendToAll(data, DeliveryMethod.ReliableOrdered);
        }

        public void OnPeerConnected(NetPeer peer)
        {
            var connectionId = Guid.NewGuid();

            _activeConnections[peer.Id] = connectionId;

            var remoteEndpoint = $"{peer.Address}:{peer.Port}";

            var joinEvent = new PlayerJoinedEvent(connectionId, remoteEndpoint);

            _inputQueue.Enqueue(joinEvent);

            _logger.LogInformation(
                "Peer {PeerId} connected from {EndPoint}. Assigned ConnectionId: {ConnectionId}",
                peer.Id,
                remoteEndpoint,
                connectionId
            );
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            if (_activeConnections.TryRemove(peer.Id, out var connectionId))
            {
                _logger.LogInformation(
                    "Peer disconnected. ConnectionId: {ConnectionId}. Reason: {Reason}",
                    connectionId,
                    disconnectInfo.Reason
                );

                _inputQueue.Enqueue(
                    new PlayerLeftEvent(connectionId, disconnectInfo.Reason.ToString())
                );
            }
        }

        public void OnNetworkReceive(
            NetPeer peer,
            NetPacketReader reader,
            byte channelNumber,
            DeliveryMethod deliveryMethod
        )
        {
            if (!_activeConnections.TryGetValue(peer.Id, out var connectionId))
                return;

            byte[] payload = reader.GetRemainingBytes();
            var packet = _serializer.Deserialize<GamePacket>(payload);

            IGameAction action = packet.Action switch
            {
                ActionType.CastSkill => new CastSkillAction(packet.ToAbilityContext()),
                ActionType.Move => PlayerMoveAction.FromVector(
                    connectionId,
                    packet.TargetPos,
                    yaw: packet.Yaw
                ),
                _ => throw new InvalidOperationException($"Unknown packet action: {packet.Action}"),
            };

            _inputQueue.Enqueue(action);
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            // 1. Log with high severity - this is an infrastructure failure
            _logger.LogError("Network error at {EndPoint}: {ErrorCode}", endPoint, socketError);

            // 2. Decide if you need to act
            // Some errors are fatal (e.g., AccessDenied), others are transient.
            // LiteNetLib will usually handle the connection teardown for you.

            // 3. Optional: Metrics (Advanced)
            // If you are using Prometheus or similar, increment a counter here
            // so your dashboard alerts you to spikes in socket errors.
        }

        public void OnNetworkReceiveUnconnected(
            IPEndPoint remoteEndPoint,
            NetPacketReader reader,
            UnconnectedMessageType messageType
        )
        {
            throw new NotImplementedException();
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            throw new NotImplementedException();
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
