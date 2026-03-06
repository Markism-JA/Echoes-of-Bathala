using Echoes.Application.Common.Serialization;
using MessagePack;

namespace Echoes.Infrastructure.Common.Serialization;

public class MessagePackSerializationService : ISerializer
{
    private readonly MessagePackSerializerOptions _options =
        MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

    public byte[] Serialize<T>(T value)
    {
        return MessagePackSerializer.Serialize(value, _options);
    }

    public T? Deserialize<T>(byte[] data)
    {
        return MessagePackSerializer.Deserialize<T>(data, _options);
    }
}
