namespace Echoes.Application.Core.Abstractions;

public interface ISerializer
{
    /// <summary>
    /// Converts a strongly-typed object into a byte array for storage or transmission.
    /// </summary>
    public byte[] Serialize<T>(T value);

    /// <summary>
    /// Converts a memory buffer back into a strongly-typed object.
    /// Uses <see cref="ReadOnlySpan{T}"/> for zero-copy deserialization.
    /// </summary>
    public T? Deserialize<T>(ReadOnlyMemory<byte> data);
}
