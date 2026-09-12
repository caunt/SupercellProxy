using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Server;

/// <summary>Configures the future game server endpoint.</summary>
public sealed class ServerOptions
{
    /// <summary>Gets or sets the local listener IP address.</summary>
    public string ListenAddress { get; set; } = ConnectionAddressResolver.DefaultListenHost;

    /// <summary>Gets or sets the local port; zero requests an ephemeral port.</summary>
    public int ListenPort { get; set; } = ConnectionAddressResolver.DefaultPort;
}
