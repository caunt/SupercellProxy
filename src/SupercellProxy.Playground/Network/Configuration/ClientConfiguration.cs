namespace SupercellProxy.Playground.Network.Configuration;

/// <summary>
/// Defines the <c language="csharp">ClientConfiguration</c> settings.
/// </summary>
/// <param name="UpstreamHost">The <c language="csharp">UpstreamHost</c> value.</param>
/// <param name="UpstreamPort">The <c language="csharp">UpstreamPort</c> value.</param>
/// <param name="Protocol">The <c language="csharp">Protocol</c> value.</param>
/// <param name="SessionPath">The <c language="csharp">SessionPath</c> value.</param>
/// <param name="BootstrapFingerprintSha">The <c language="csharp">BootstrapFingerprintSha</c> value.</param>
internal sealed record ClientConfiguration(
    string UpstreamHost,
    int UpstreamPort,
    ProtocolConfiguration Protocol,
    string? SessionPath = null,
    // Deliberately stale bootstrap value: it must only provoke the single expected
    // OutdatedContent LoginFailed. The current fingerprint is detected from that response.
    string? BootstrapFingerprintSha = null
);
