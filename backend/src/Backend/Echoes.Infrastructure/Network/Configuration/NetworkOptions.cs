namespace Echoes.Infrastructure.Network.Configuration;

/// <summary>
/// Configuration settings for the LiteNetLib transport layer.
/// </summary>
public class NetworkOptions
{
    public const string SectionName = "Network";

    /// <summary>
    /// The UDP port the server will listen on.
    /// </summary>
    public int Port { get; set; } = 9050;

    /// <summary>
    /// The local IP address to bind to. "Any" or "0.0.0.0" listens on all interfaces (Any device on LAN can connect).
    /// </summary>
    public string BindAddress { get; set; } = "Any";

    /// <summary>
    /// A shared secret string that clients must provide to establish a connection.
    /// Acts as a first-line defense against unauthorized clients.
    /// </summary>
    public string ConnectionKey { get; set; } = null!;

    /// <summary>
    /// Maximum number of concurrent physical connections allowed.
    /// </summary>
    public int MaxConnections { get; set; } = 1000;

    /// <summary>
    /// How long (in milliseconds) the server waits before timing out an inactive peer.
    /// </summary>
    public int DisconnectTimeoutMs { get; set; } = 15000;

    /// <summary>
    /// The maximum number of incoming packets allowed per second from a single peer.
    /// Used to prevent flooding and basic Denial of Service (DoS) attempts.
    /// </summary>
    public int MaxPacketPerSecond { get; set; } = 100;

    /// <summary>
    /// The maximum allowed size (in bytes) for a single network packet.
    /// Default is 8KB (8192 bytes). Packets exceeding this will be rejected.
    /// </summary>
    public int MaxPacketPerSize { get; set; } = 1024 * 8;
}
