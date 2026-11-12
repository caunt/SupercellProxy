using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Client;

/// <summary>
/// Everything a <see cref="ProtocolClient"/> needs to reach the game server and log in as the player.
/// </summary>
/// <param name="UpstreamHost">Game server host name or address the client connects to.</param>
/// <param name="UpstreamPort">TCP port of the game server, normally 9339.</param>
/// <param name="Protocol">An explicit version override; <see langword="null"/> resolves the latest version from KEYS.md.</param>
/// <param name="SessionTokenProvider">
/// Supplies the Supercell ID session token for a logged-in player. It is called with <see langword="false"/> for the token it holds; when the server
/// rejects that token as invalid, it is called once more with <see langword="true"/> and must return a freshly issued one. Leave <see langword="null"/> for anonymous
/// accounts.
/// </param>
/// <param name="BootstrapFingerprintSha">
/// Asset bundle hash sent on the first login so the server answers with an out-of-date-content failure that carries the current asset
/// manifest. Null sends an empty hash, which the server always treats as out of date.
/// </param>
/// <param name="AssetDirectory">The optional root directory containing versioned game assets.</param>
public sealed record ClientConfiguration(string UpstreamHost,
    int UpstreamPort,
    ProtocolConfiguration? Protocol = null,
    Func<bool, CancellationToken, Task<SessionTokenData>>? SessionTokenProvider = null,
    // Deliberately stale bootstrap value: it must only provoke the single expected
    // OutdatedContent LoginFailed. The current fingerprint is detected from that response.
    string? BootstrapFingerprintSha = null,
    string? AssetDirectory = null)
{
    /// <summary>Gets or sets the saved anonymous account an own-farm connection logs in as.</summary>
    /// <value>The account id of an entry in the anonymous-account pool, or <see langword="null"/> (the default) to lease any idle account or reserve a new one; with an id set, a rejected credential fails the login instead of moving on to another account.</value>
    public long? AnonymousAccountId { get; init; }

    /// <summary>Gets or sets an optional observer of received plaintext and successfully written plaintext frames, which sees them without changing their contents.</summary>
    /// <value>A callback given the frame's direction, its raw container and the decoded message (<see langword="null"/> when a raw container is sent without a typed message); <see langword="null"/> by default, meaning nothing is observed.</value>
    public Func<MessageDirection, MessageContainer, IMessage?, ValueTask>? ObserveMessage { get; init; }
}
