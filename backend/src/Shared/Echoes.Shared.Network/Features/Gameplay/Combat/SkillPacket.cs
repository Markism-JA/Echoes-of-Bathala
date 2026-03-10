using System;
using Echoes.Shared.Network.Common.Abstractions;
using Echoes.Shared.Network.Common.Protocol;
using MemoryPack;

namespace Echoes.Shared.Network.Features.Gameplay.Combat;

/// <summary>
/// Represents a client-side request to execute a combat ability.
/// </summary>
/// <remarks>
/// Upon receiving this packet, the server should perform validation checks (e.g., cooldown status,
/// distance to target, resource availability) before broadcasting the effect to other clients.
/// </remarks>
[MemoryPackable]
public partial record SkillPacket(
    Guid SenderId,
    int SkillId,
    Guid TargetId,
    long Timestamp,
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
    /// Gets the unique identifier for the skill being cast.
    /// This should correspond to a definition in the server's ability registry.
    /// </summary>
    public int SkillId { get; init; } = SkillId;

    /// <summary>
    /// Gets the unique identifier of the target entity.
    /// </summary>
    /// <value>
    /// The <see cref="Guid"/> of the target. Use <see cref="Guid.Empty"/>
    /// if the skill is self-targeted or ground-targeted.
    /// </value>
    public Guid TargetId { get; init; } = TargetId;
}
