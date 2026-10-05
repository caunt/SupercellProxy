using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Client;

/// <summary>
/// Defines the <c language="csharp">ClientConfiguration</c> settings.
/// </summary>
/// <param name="UpstreamHost">The <c language="csharp">UpstreamHost</c> value.</param>
/// <param name="UpstreamPort">The <c language="csharp">UpstreamPort</c> value.</param>
/// <param name="Protocol">An explicit version override; null resolves the latest version from KEYS.md.</param>
/// <param name="SessionTokenProvider"></param>
/// <param name="BootstrapFingerprintSha">The <c language="csharp">BootstrapFingerprintSha</c> value.</param>
/// <param name="AssetDirectory">The optional root directory containing versioned game assets.</param>
public sealed record ClientConfiguration(
    string UpstreamHost,
    int UpstreamPort,
    ProtocolConfiguration? Protocol = null,
    Func<bool, CancellationToken, Task<SessionTokenData>>? SessionTokenProvider = null,
    // Deliberately stale bootstrap value: it must only provoke the single expected
    // OutdatedContent LoginFailed. The current fingerprint is detected from that response.
    string? BootstrapFingerprintSha = null,
    string? AssetDirectory = null
)
{
    /// <summary>Optionally observes received plaintext and successfully written plaintext frames without changing their contents.</summary>
    public Func<MessageDirection, MessageContainer, IMessage?, ValueTask>? ObserveMessage { get; init; }
}
