using Microsoft.Extensions.Logging;

using SupercellProxy.Networking.Cryptography;
using SupercellProxy.Networking.Transport;
using SupercellProxy.Networking.Sessions.Anonymous;

namespace SupercellProxy.Networking.Client;

/// <summary>Creates independent protocol connections using shared DI infrastructure.</summary>
/// <param name="webClients">Creates the named <c>ScClient</c> HTTP client each new protocol client gets for asset downloads and disposes on its own disposal.</param>
/// <param name="timeProvider">Clock passed to every client for keep-alives and traffic averaging.</param>
/// <param name="loggerFactory">Creates each client's <see cref="ILogger{TCategoryName}"/>.</param>
/// <param name="serverKeys">Server public key source shared by all clients.</param>
/// <param name="anonymousAccounts">Saved anonymous-account pool shared by all online clients, or <see langword="null"/> when anonymous logins should not be saved.</param>
public sealed class ProtocolClientFactory(IHttpClientFactory webClients,
    TimeProvider timeProvider,
    ILoggerFactory loggerFactory,
    IServerPublicKeySource serverKeys,
    IAnonymousAccountPool? anonymousAccounts = null)
{
    private const string ClientWebClientName = "ScClient";

    /// <summary>Creates an online client; the caller owns its lifetime.</summary>
    /// <param name="configuration">The server address, protocol version, session token source and asset directory the new client logs in with.</param>
    /// <returns>A new, not yet connected <see cref="ProtocolClient"/> with its own HTTP client and the factory's clock, logger, server key source and anonymous-account pool.</returns>
    public ProtocolClient Create(ClientConfiguration configuration)
    {
        return new(configuration,
            webClients.CreateClient(ClientWebClientName),
            timeProvider,
            loggerFactory.CreateLogger<ProtocolClient>(),
            serverKeys,
            anonymousAccounts);
    }

    /// <summary>Creates a client over an existing stream; the caller owns its lifetime.</summary>
    /// <param name="stream">The already open or replayed message stream the client sends and receives on; it must not be <see langword="null"/>.</param>
    /// <returns>A new <see cref="ProtocolClient"/> bound to <paramref name="stream"/> that has no login configuration, so it cannot connect on its own.</returns>
    public ProtocolClient Create(MessageStream stream)
    {
        return new(stream, webClients.CreateClient(ClientWebClientName), timeProvider, loggerFactory.CreateLogger<ProtocolClient>(), serverKeys);
    }
}
