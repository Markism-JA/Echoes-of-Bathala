using Echoes.Application.Common.Abstractions;
using MessagePack;

namespace Echoes.Infrastructure.Serialization;

/// <summary>
/// A binary implementation of <see cref="ISerializer"/> using the MessagePack protocol.
/// </summary>
/// <remarks>
/// This service utilizes LZ4 Block Array compression to minimize bandwidth usage.
/// It offers a robust balance between the extreme speed of MemoryPack and the
/// flexibility/interoperability of standard binary formats.
/// </remarks>
public class MessagePackSerializationService : ISerializer
{
    /// <summary>
    /// Configuration options using Standard resolver with LZ4 Compression enabled.
    /// </summary>
    private readonly MessagePackSerializerOptions _options =
        MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

    /// <inheritdoc />
    /// <remarks>
    /// Serializes the object into a compressed MessagePack binary format.
    /// Note: Target types should be decorated with [MessagePackObject] and [Key] attributes.
    /// </remarks>
    public byte[] Serialize<T>(T value)
    {
        return MessagePackSerializer.Serialize(value, _options);
    }

    /// <inheritdoc />
    /// <remarks>
    /// Deserializes the <see cref="ReadOnlyMemory{T}"/> directly.
    /// MessagePack provides native support for Memory-based buffers, allowing for
    /// efficient handling of network-received data.
    /// </remarks>
    public T? Deserialize<T>(ReadOnlyMemory<byte> data)
    {
        // MessagePack has an optimized overload that accepts ReadOnlyMemory<byte> directly
        return MessagePackSerializer.Deserialize<T>(data, _options);
    }
}
