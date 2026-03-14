using System.Text.Json;
using Echoes.Application.Common.Abstractions;

namespace Echoes.Infrastructure.Serialization
{
    public class JsonSerializationService(JsonSerializerOptions options) : ISerializer
    {
        public byte[] Serialize<T>(T value)
        {
            var json = JsonSerializer.Serialize(value, options);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

        public T? Deserialize<T>(byte[] data)
        {
            var json = System.Text.Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}
