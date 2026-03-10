using System;

namespace Echoes.Shared.Network.Common.Abstractions;

/// <summary>
/// Defines the base contract for all network packets within the Client and Game Server system.
/// Implementing classes must provide mandatory metadata for routing and synchronization.
/// </summary>
public interface IPacket
{
    /// <summary>
    /// Gets the unique identifier of the entity or client that originated this packet.
    /// Used for packet routing, authentication, and connection mapping.
    /// </summary>
    public Guid SenderId { get; }

    /// <summary>
    /// Gets the high-resolution timestamp of when the packet was generated.
    /// Used for sequencing, jitter buffering, and lag compensation calculations.
    /// </summary>
    public long Timestamp { get; }

    /// <summary>
    /// Gets the correlation identifier used to match outgoing requests with their corresponding incoming responses.
    /// </summary>
    /// <remarks>
    /// When the client initiates a request, it assigns a unique ID. The server is expected
    /// to echo this exact ID in the response packet. This allows the client to asynchronously
    /// resolve the correct task or callback, even if multiple requests are in-flight simultaneously.
    /// </remarks>
    public int CorrelationId { get; }
}
