using Echoes.Application.Core.Abstractions;
using MemoryPack;

namespace Echoes.Infrastructure.Serialization
{
    public class MemoryPackSerializationService : ISerializer
    {
        public T? Deserialize<T>(ReadOnlyMemory<byte> data)
        {
            return MemoryPackSerializer.Deserialize<T>(data.Span);
        }

        public byte[] Serialize<T>(T value)
        {
            return MemoryPackSerializer.Serialize(value);
        }
    }
}
