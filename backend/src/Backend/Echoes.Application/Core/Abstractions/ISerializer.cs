namespace Echoes.Application.Core.Abstractions;

public interface ISerializer
{
    /// <summary>
    /// Converts a strongly-typed object into a byte array for storage or transmission.
    /// </summary>
    public byte[] Serialize<T>(T value);

    /// <summary>
    /// Converts a byte array back into a strongly-typed object.
    /// </summary>
    public T? Deserialize<T>(byte[] data);
}
