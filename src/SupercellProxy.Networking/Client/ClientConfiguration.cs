using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Client;

/// <summary>
/// Everything a <see cref="ProtocolClient"/> needs to reach the game server and log in as the player.
/// </summary>
/// <param name="UpstreamHost">Game server host name or address the client connects to.</param>
/// <param name="UpstreamPort">TCP port of the game server, normally 9339.</param>
/// <param name="Protocol">An explicit version override; null resolves the latest version from KEYS.md.</param>
/// <param name="SessionTokenProvider">
/// Supplies the Supercell ID session token for a logged-in player. It is called with false for the token it holds; when the server
/// rejects that token as invalid, it is called once more with true and must return a freshly issued one. Leave null for guest
/// accounts.
/// </param>
/// <param name="BootstrapFingerprintSha">
/// Asset bundle hash sent on the first login so the server answers with an out-of-date-content failure that carries the current asset
/// manifest. Null sends an empty hash, which the server always treats as out of date.
/// </param>
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
    /// <summary>Selects a saved anonymous account for an own-farm connection.</summary>
    public long? AnonymousAccountId { get; init; }

    /// <summary>Optionally observes received plaintext and successfully written plaintext frames without changing their contents.</summary>
    public Func<MessageDirection, MessageContainer, IMessage?, ValueTask>? ObserveMessage { get; init; }
}
