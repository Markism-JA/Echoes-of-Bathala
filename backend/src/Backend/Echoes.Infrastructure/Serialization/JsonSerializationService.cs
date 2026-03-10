using System.Text.Json;
using Echoes.Application.Core.Abstractions;

namespace Echoes.Infrastructure.Serialization
{
    public class JsonSerializationService(JsonSerializerOptions options) : ISerializer
    {
        public byte[] Serialize<T>(T value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value, options);
        }

        public T? Deserialize<T>(ReadOnlyMemory<byte> data)
        {
            return JsonSerializer.Deserialize<T>(data.Span, options);
        }
    }
}
