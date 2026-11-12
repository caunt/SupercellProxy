using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Client;

/// <summary>Configures an authenticated protocol client.</summary>
public sealed class ClientOptions
{
    /// <summary>Gets or sets the versioned asset cache root.</summary>
    /// <value>The directory under which downloaded assets are kept as <c>&lt;version&gt;/&lt;fingerprint&gt;</c>; <see langword="null"/> (the default) uses the user data asset directory.</value>
    public string? AssetDirectory { get; set; }

    /// <summary>Gets or sets the optional stale bootstrap fingerprint.</summary>
    /// <value>The asset hash sent on the first login to provoke the out-of-date-content failure that carries the current manifest; <see langword="null"/> (the default) sends an empty hash.</value>
    public string? BootstrapFingerprintSha { get; set; }
    /// <summary>Gets or sets an explicit version override; <see langword="null"/> uses the latest version from KEYS.md.</summary>
    /// <value>The client version and server key to log in with, or <see langword="null"/> (the default) to pick the newest configured one.</value>
    public ProtocolConfiguration? Protocol { get; set; }

    /// <summary>Gets or sets the session-token source; its boolean requests a forced refresh.</summary>
    /// <value>A callback returning the Supercell ID session token, asked once more with <see langword="true"/> after the server rejects the token; <see langword="null"/> (the default) logs in anonymously.</value>
    public Func<bool, CancellationToken, Task<SessionTokenData>>? SessionTokenProvider { get; set; }

    /// <summary>Gets or sets the upstream hostname or IP address.</summary>
    /// <value>The game server to connect to; defaults to <c>game.haydaygame.com</c>.</value>
    public string UpstreamHost { get; set; } = ConnectionAddressResolver.DefaultUpstreamHost;

    /// <summary>Gets or sets the upstream TCP port.</summary>
    /// <value>The game server port; defaults to 9339.</value>
    public int UpstreamPort { get; set; } = ConnectionAddressResolver.DefaultPort;

    internal ClientConfiguration ToConfiguration()
    {
        return new(UpstreamHost, UpstreamPort, Protocol, SessionTokenProvider, BootstrapFingerprintSha, AssetDirectory);
    }
}
