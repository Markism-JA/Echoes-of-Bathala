using Echoes.Application.Common.Abstractions;
using MemoryPack;

namespace Echoes.Infrastructure.Serialization;

/// <summary>
/// A high-performance binary implementation of <see cref="ISerializer"/> using MemoryPack.
/// </summary>
/// <remarks>
/// This service leverages zero-allocation code generation for maximum throughput.
/// It is the recommended serializer for the "hot path (Simulation Layer)" in game networking
/// due to its minimal CPU overhead and compact binary footprint.
/// </remarks>
public class MemoryPackSerializationService : ISerializer
{
    /// <inheritdoc />
    /// <remarks>
    /// Produces a tightly packed binary array.
    /// Note: Ensure that types passed here are decorated with the [MemoryPackable] attribute.
    /// </remarks>
    public byte[] Serialize<T>(T value)
    {
        return MemoryPackSerializer.Serialize(value);
    }

    /// <inheritdoc />
    /// <remarks>
    /// Produces a tightly packed binary array.
    /// Note: Ensure that types passed here are decorated with the [MemoryPackable] attribute.
    /// </remarks>
    public T? Deserialize<T>(ReadOnlyMemory<byte> data)
    {
        return MemoryPackSerializer.Deserialize<T>(data.Span);
    }
}
