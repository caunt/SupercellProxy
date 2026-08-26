namespace SupercellProxy.Playground.Network.Configuration;

/// <summary>
/// Defines the <c>ClientConfiguration</c> settings.
/// </summary>
/// <param name="UpstreamHost">The <c>UpstreamHost</c> value.</param>
/// <param name="UpstreamPort">The <c>UpstreamPort</c> value.</param>
/// <param name="Protocol">The <c>Protocol</c> value.</param>
/// <param name="SessionPath">The <c>SessionPath</c> value.</param>
/// <param name="BootstrapFingerprintSha">The <c>BootstrapFingerprintSha</c> value.</param>
public record ClientConfiguration(
    string UpstreamHost,
    int UpstreamPort,
    ProtocolConfiguration Protocol,
    string? SessionPath = null,
    // Deliberately stale bootstrap value: it must only provoke the single expected
    // OutdatedContent LoginFailed. The current fingerprint is detected from that response.
    string? BootstrapFingerprintSha = null
);
