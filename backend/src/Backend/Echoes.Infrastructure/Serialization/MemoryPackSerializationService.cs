using Echoes.Application.Core.Abstractions;
using MemoryPack;

namespace Echoes.Infrastructure.Serialization
{
    public class MemoryPackSerializationService : ISerializer
    {
        public T? Deserialize<T>(byte[] data)
        {
            return MemoryPackSerializer.Deserialize<T>(data);
        }

        public byte[] Serialize<T>(T value)
        {
            return MemoryPackSerializer.Serialize(value);
        }
    }
}
