namespace Echoes.Infrastructure.Networking.Configuration;

/// <summary>
/// Configuration settings for the network transport layer.
/// </summary>
/// <remarks>
/// This class is designed to be used with the .NET Options Pattern (<see cref="Microsoft.Extensions.Options.IOptions{TOptions}"/>),
/// allowing settings to be bound automatically from <c>appsettings.json</c>, environment variables, or command-line arguments.
/// </remarks>
public class NetworkOptions
{
    /// <summary>
    /// The UDP port the server will bind to for incoming connections.
    /// </summary>
    /// <value>Defaults to <c>9000</c>.</value>
    public int Port { get; set; } = 9000;
}
