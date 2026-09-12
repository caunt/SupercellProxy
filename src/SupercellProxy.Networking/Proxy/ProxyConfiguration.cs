using SupercellProxy.Networking.Protocol.Authentication;

namespace SupercellProxy.Networking.Proxy;

/// <summary>
/// Defines the <c language="csharp">ProxyConfiguration</c> settings.
/// </summary>
public sealed record ProxyConfiguration(
    string UpstreamHost,
    int UpstreamPort,
    string ListenAddress,
    int ListenPort,
    ProtocolConfiguration Protocol,
    string? SessionPath = null,
    string? CaptureDirectory = null,
    string? AssetDirectory = null
);
