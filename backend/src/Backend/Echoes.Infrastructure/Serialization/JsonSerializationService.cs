using System.Text.Json;
using Echoes.Application.Common.Abstractions;

namespace Echoes.Infrastructure.Serialization
{
    /// <summary>
    /// A JSON-based implementation of <see cref="ISerializer"/> using System.Text.Json.
    /// </summary>
    /// <remarks>
    /// This service is ideal for human-readable debugging or cross-platform compatibility
    /// where binary serialization (like MemoryPack) is not required.
    /// </remarks>
    public class JsonSerializationService(JsonSerializerOptions options) : ISerializer
    {
        /// <inheritdoc />
        /// <remarks>
        /// Serializes directly to a UTF-8 encoded byte array to minimize string allocation overhead.
        /// </remarks>
        public byte[] Serialize<T>(T value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value, options);
        }

        /// <inheritdoc />
        /// <remarks>
        /// Deserializes by accessing the underlying <see cref="ReadOnlySpan{T}"/> of the provided memory.
        /// This provides the high-performance benefits of Span-based parsing while maintaining
        /// the interface's <see cref="ReadOnlyMemory{T}"/> contract.
        /// </remarks>
        public T? Deserialize<T>(ReadOnlyMemory<byte> data)
        {
            return JsonSerializer.Deserialize<T>(data.Span, options);
        }
    }
}
