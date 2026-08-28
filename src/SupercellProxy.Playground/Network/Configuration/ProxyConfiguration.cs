namespace SupercellProxy.Playground.Network.Configuration;

/// <summary>
/// Defines the <c language="csharp">ProxyConfiguration</c> settings.
/// </summary>
internal sealed record ProxyConfiguration(
    string UpstreamHost,
    int UpstreamPort,
    string ListenAddress,
    int ListenPort,
    ProtocolConfiguration Protocol
);
