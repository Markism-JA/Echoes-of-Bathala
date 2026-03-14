namespace Echoes.Application.Common.Abstractions;

/// <summary>
/// Defines a contract for high-performance data serialization and deserialization.
/// </summary>
/// <remarks>
/// <strong>Registration Note:</strong> Implementations of this interface are intended to be
/// stateless and thread-safe. They should generally be registered as <b>Singletons</b>.
/// In systems with multiple serialization formats, use <b>Keyed Services</b> to differentiate
/// between JSON, MemoryPack, etc.
/// </remarks>
public interface ISerializer
{
    /// <summary>
    /// Converts a strongly-typed object into a byte array for storage or transmission.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="value">The object instance to convert.</param>
    /// <returns>A byte array representing the serialized data.</returns>
    public byte[] Serialize<T>(T value);

    /// <summary>
    /// Converts a memory buffer back into a strongly-typed object.
    /// </summary>
    /// <typeparam name="T">The target type for deserialization.</typeparam>
    /// <param name="data">
    /// The source buffer as <see cref="ReadOnlyMemory{T}"/>.
    /// This allows for efficient, sliceable access to the underlying data without mandatory copying.
    /// </param>
    /// <returns>The deserialized object of type <typeparamref name="T"/>, or null if deserialization fails.</returns>
    /// <remarks>
    /// ReadOnlyMemory is used instead of ReadOnlySpan to ensure compatibility with
    /// asynchronous pipelines and modern serializers (MemoryPack, MessagePack, JSON)
    /// that require the buffer to persist across stack frames.
    /// </remarks>
    public T? Deserialize<T>(ReadOnlyMemory<byte> data);
}
