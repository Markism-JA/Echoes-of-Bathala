using System;
using Echoes.Shared.Network.Common.Abstractions;
using Echoes.Shared.Network.Common.Protocol;
using MemoryPack;

namespace Echoes.Shared.Network.Features.ServerSession;

/// <summary>
/// A response packet sent by the server to inform the client of the outcome of an <see cref="ServerSession.EnterGamePacket"/> request.
/// </summary>
[MemoryPackable]
public partial record EnterGameResponsePacket(
    Guid SenderId,
    long Timestamp,
    bool Success,
    EnterGameResult Result,
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
    /// Gets a value indicating whether the entry request was processed successfully.
    /// </summary>
    public bool Success { get; init; } = Success;

    /// <summary>
    /// Gets the detailed result code of the entry attempt.
    /// </summary>
    public EnterGameResult Result { get; init; } = Result;
}
