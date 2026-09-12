using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Proxy;

/// <summary>Configures the proxy listener, upstream connection, and optional recording.</summary>
public sealed class ProxyOptions
{

    /// <summary>Gets or sets the local fingerprint directory used by codecs.</summary>
    public string? AssetDirectory { get; set; }

    /// <summary>Gets or sets the capture directory; null disables recording.</summary>
    public string? CaptureDirectory { get; set; }
    /// <summary>Gets or sets the upstream hostname or IP address.</summary>
    public string UpstreamHost { get; set; } = ConnectionAddressResolver.DefaultUpstreamHost;

    /// <summary>Gets or sets the upstream TCP port.</summary>
    public int UpstreamPort { get; set; } = ConnectionAddressResolver.DefaultPort;

    /// <summary>Gets or sets the local listener IP address.</summary>
    public string ListenAddress { get; set; } = ConnectionAddressResolver.DefaultListenHost;

    /// <summary>Gets or sets the optional ledger account tag to inject.</summary>
    public string? SessionAccountIdentifier { get; set; }

    /// <summary>Gets or sets the optional client session ledger path.</summary>
    public string? SessionLedgerPath { get; set; }

    /// <summary>Gets or sets the local port; zero requests an ephemeral port.</summary>
    public int ListenPort { get; set; } = ConnectionAddressResolver.DefaultPort;

    /// <summary>Gets or sets the protocol version.</summary>
    public ProtocolConfiguration Protocol { get; set; } = ProtocolConfiguration.Current with { };

    internal ProxyConfiguration ToConfiguration()
    {
        return new(
            UpstreamHost,
            UpstreamPort,
            ListenAddress,
            ListenPort,
            Protocol,
            SessionAccountIdentifier,
            SessionLedgerPath,
            CaptureDirectory,
            AssetDirectory
        );
    }
}
