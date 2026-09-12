using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Client;

/// <summary>Configures an authenticated protocol client.</summary>
public sealed class ClientOptions
{

    /// <summary>Gets or sets the versioned asset cache root.</summary>
    public string? AssetDirectory { get; set; }

    /// <summary>Gets or sets the optional stale bootstrap fingerprint.</summary>
    public string? BootstrapFingerprintSha { get; set; }
    /// <summary>Gets or sets the upstream hostname or IP address.</summary>
    public string UpstreamHost { get; set; } = ConnectionAddressResolver.DefaultUpstreamHost;

    /// <summary>Gets or sets the optional account session file.</summary>
    public string? SessionPath { get; set; }

    /// <summary>Gets or sets the upstream TCP port.</summary>
    public int UpstreamPort { get; set; } = ConnectionAddressResolver.DefaultPort;

    /// <summary>Gets or sets the protocol version advertised during authentication.</summary>
    public ProtocolConfiguration Protocol { get; set; } = ProtocolConfiguration.Current with { };

    internal ClientConfiguration ToConfiguration()
    {
        return new(UpstreamHost, UpstreamPort, Protocol, SessionPath, BootstrapFingerprintSha, AssetDirectory);
    }
}
