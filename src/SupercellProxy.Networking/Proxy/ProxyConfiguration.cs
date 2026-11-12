using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Sessions.Anonymous;
using SupercellProxy.Networking.Protocol.Authentication;

namespace SupercellProxy.Networking.Proxy;

/// <summary>
/// Defines the <c language="csharp">ProxyConfiguration</c> settings.
/// </summary>
/// <param name="UpstreamHost">Host name or IP address of the real game server each accepted client is connected to.</param>
/// <param name="UpstreamPort">TCP port of the real game server, normally 9339.</param>
/// <param name="ListenAddress">Local IP address the listener binds to, as text; it must parse as an address, so host names are not accepted.</param>
/// <param name="ListenPort">Local TCP port to accept game clients on; 0 lets the system pick a free port.</param>
/// <param name="Protocol">An explicit game and key version for tools that look up accounts, or <see langword="null"/> for the latest version from KEYS.md; the proxy itself takes the version from the client's handshake.</param>
/// <param name="SessionTokenProvider">When set, the client's login is rewritten to use this Supercell ID session token, and it is called with <see langword="true"/> to refresh a token the server rejects; <see langword="null"/> forwards the client's own credentials.</param>
/// <param name="CaptureDirectory">Root folder for frame captures, or <see langword="null"/> to capture nothing.</param>
/// <param name="AssetDirectory">Folder of versioned game assets loaded to decode commands and retained alongside captures, or <see langword="null"/> to use the resolver given to the proxy.</param>
/// <param name="AnonymousAccount">Lease of a saved anonymous account whose id and pass token replace the client's login credentials, and whose saved token is updated when the server accepts it; <see langword="null"/> to leave anonymous logins alone.</param>
public sealed record ProxyConfiguration(string UpstreamHost,
    int UpstreamPort,
    string ListenAddress,
    int ListenPort,
    ProtocolConfiguration? Protocol = null,
    Func<bool, CancellationToken, Task<SessionTokenData>>? SessionTokenProvider = null,
    string? CaptureDirectory = null,
    string? AssetDirectory = null,
    AnonymousAccountLease? AnonymousAccount = null);
