using System;
using Echoes.Shared.Network.Common.Abstractions;
using Echoes.Shared.Network.Common.Protocol;
using MemoryPack;

namespace Echoes.Shared.Network.Features.ServerSession;

/// <summary>
/// A request packet sent by the client to initiate entry into the game world.
/// </summary>
/// <remarks>
/// This packet must be sent immediately upon establishing a connection.
/// The server validates the <see cref="TicketGuid"/> against the authentication service
/// before allowing the client to proceed.
/// </remarks>
[MemoryPackable]
public partial record EnterGamePacket(
    Guid SenderId,
    long Timestamp,
    Guid TicketGuid,
    int CorrelationId
) : IPacketUnion
{
    /// <inheritdoc cref="IPacket.SenderId" />
    public Guid SenderId { get; init; } = SenderId;

    /// <inheritdoc cref="IPacket.Timestamp" />
    public long Timestamp { get; init; } = Timestamp;

    /// <inheritdoc cref="IPacket.CorrelationId" />
    public int CorrelationId { get; init; } = CorrelationId;

    /// <summary>
    /// Gets the unique session ticket obtained from the Authentication API.
    /// Used by the game server to verify the client's identity.
    /// </summary>
    public Guid TicketGuid { get; init; } = TicketGuid;
}
