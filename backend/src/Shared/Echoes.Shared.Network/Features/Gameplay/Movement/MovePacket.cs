using System;
using System.Numerics;
using Echoes.Shared.Network.Common.Abstractions;
using Echoes.Shared.Network.Common.Protocol;
using MemoryPack;

namespace Echoes.Shared.Network.Features.Gameplay.Movement;

[MemoryPackable]
public partial record MovePacket(Guid SenderId, long Timestamp, Vector3 Position, int CorrelationId)
    : IPacketUnion
{
    /// <inheritdoc cref="IPacket.SenderId"/>.>
    public Guid SenderId { get; init; } = SenderId;

    /// <inheritdoc cref="IPacket.Timestamp" />
    public long Timestamp { get; init; } = Timestamp;

    /// <summary>
    /// Gets the world-space position vector of the entity.
    /// </summary>
    /// <value>
    /// A <see cref="Vector3"/> representing the current (x, y, z) coordinates
    /// relative to the game world origin.
    /// </value>
    public Vector3 Position { get; init; } = Position;

    /// <inheritdoc cref="IPacket.CorrelationId"/>
    public int CorrelationId { get; init; } = CorrelationId;
}
