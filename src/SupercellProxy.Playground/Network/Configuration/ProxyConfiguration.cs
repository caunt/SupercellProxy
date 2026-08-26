namespace SupercellProxy.Playground.Network.Configuration;

/// <summary>
/// Defines the <c>ProxyConfiguration</c> settings.
/// </summary>
public record ProxyConfiguration(
    string UpstreamHost,
    int UpstreamPort,
    string ListenAddress,
    int ListenPort,
    ProtocolConfiguration Protocol
);
