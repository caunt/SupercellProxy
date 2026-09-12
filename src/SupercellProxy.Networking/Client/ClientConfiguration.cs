using SupercellProxy.Networking.Protocol.Authentication;

namespace SupercellProxy.Networking.Client;

/// <summary>
/// Defines the <c language="csharp">ClientConfiguration</c> settings.
/// </summary>
/// <param name="UpstreamHost">The <c language="csharp">UpstreamHost</c> value.</param>
/// <param name="UpstreamPort">The <c language="csharp">UpstreamPort</c> value.</param>
/// <param name="Protocol">The <c language="csharp">Protocol</c> value.</param>
/// <param name="SessionAccountIdentifier">The account tag selected from the session ledger.</param>
/// <param name="SessionLedgerPath">The optional client session ledger path.</param>
/// <param name="BootstrapFingerprintSha">The <c language="csharp">BootstrapFingerprintSha</c> value.</param>
/// <param name="AssetDirectory">The optional root directory containing versioned game assets.</param>
public sealed record ClientConfiguration(
    string UpstreamHost,
    int UpstreamPort,
    ProtocolConfiguration Protocol,
    string? SessionAccountIdentifier = null,
    string? SessionLedgerPath = null,
    // Deliberately stale bootstrap value: it must only provoke the single expected
    // OutdatedContent LoginFailed. The current fingerprint is detected from that response.
    string? BootstrapFingerprintSha = null,
    string? AssetDirectory = null
);
