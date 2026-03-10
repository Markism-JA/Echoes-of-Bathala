using Echoes.Application.Core.Abstractions;
using MessagePack;

namespace Echoes.Infrastructure.Serialization;

public class MessagePackSerializationService : ISerializer
{
    private readonly MessagePackSerializerOptions _options =
        MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

    public byte[] Serialize<T>(T value)
    {
        return MessagePackSerializer.Serialize(value, _options);
    }

    public T? Deserialize<T>(ReadOnlyMemory<byte> data)
    {
        return MessagePackSerializer.Deserialize<T>(data, _options);
    }
}
