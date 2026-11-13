using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Sessions.Anonymous;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Proxy;

/// <summary>Configures the proxy listener, upstream connection, and optional recording.</summary>
public sealed class ProxyOptions
{
    /// <summary>Gets or sets the host-owned lease used to proxy a saved anonymous account.</summary>
    /// <value>A lease whose saved credentials replace the client's login, or <see langword="null"/> (the default) to forward anonymous logins unchanged; the host keeps ownership and disposes it.</value>
    public AnonymousAccountLease? AnonymousAccount { get; set; }

    /// <summary>Gets or sets the local fingerprint directory used by codecs.</summary>
    /// <value>Root of the versioned game asset folders, loaded once when the proxy starts to decode commands; <see langword="null"/> (the default) uses the resolver passed to the proxy, if any.</value>
    public string? AssetDirectory { get; set; }

    /// <summary>Gets or sets the capture directory; <see langword="null"/> disables recording.</summary>
    /// <value>Root folder under which each connection gets its own capture folder; <see langword="null"/> by default.</value>
    public string? CaptureDirectory { get; set; }

    /// <summary>Gets or sets the local listener IP address.</summary>
    /// <value>Literal IP address to bind, since host names are not resolved; <see cref="ConnectionAddressResolver.DefaultListenHost"/> (every IPv4 interface) by default.</value>
    public string ListenAddress { get; set; } = ConnectionAddressResolver.DefaultListenHost;

    /// <summary>Gets or sets the local port; zero requests an ephemeral port.</summary>
    /// <value>TCP port game clients connect to; <see cref="ConnectionAddressResolver.DefaultPort"/> (9339) by default, and the port actually bound is reported by <see cref="ProtocolProxy.Listening"/>.</value>
    public int ListenPort { get; set; } = ConnectionAddressResolver.DefaultPort;

    /// <summary>Gets or sets an explicit version for account lookup; <see langword="null"/> uses the latest version from KEYS.md.</summary>
    /// <value>Version used by tools that log in on their own to look up accounts; the proxy's forwarded traffic keeps the client's version. <see langword="null"/> by default.</value>
    public ProtocolConfiguration? Protocol { get; set; }

    /// <summary>Gets or sets the session-token source; its boolean requests a forced refresh.</summary>
    /// <value>A callback whose token replaces the client's login credentials, called with <see langword="false"/> for each login and with <see langword="true"/> after the server rejects the token; <see langword="null"/> (the default) forwards the client's own login.</value>
    public Func<bool, CancellationToken, Task<SessionTokenData>>? SessionTokenProvider { get; set; }

    /// <summary>Gets or sets the upstream hostname or IP address.</summary>
    /// <value>Address every accepted client is forwarded to; <see cref="ConnectionAddressResolver.DefaultUpstreamHost"/>, the live Hay Day server, by default.</value>
    public string UpstreamHost { get; set; } = ConnectionAddressResolver.DefaultUpstreamHost;

    /// <summary>Gets or sets the upstream TCP port.</summary>
    /// <value>Port of the upstream server; <see cref="ConnectionAddressResolver.DefaultPort"/> (9339) by default.</value>
    public int UpstreamPort { get; set; } = ConnectionAddressResolver.DefaultPort;

    internal ProxyConfiguration ToConfiguration()
    {
        return new(UpstreamHost,
            UpstreamPort,
            ListenAddress,
            ListenPort,
            Protocol,
            SessionTokenProvider,
            CaptureDirectory,
            AssetDirectory,
            AnonymousAccount);
    }
}
