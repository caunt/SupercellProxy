using Microsoft.Extensions.Logging;

using SupercellProxy.Networking.Cryptography;
using SupercellProxy.Networking.Transport;
using SupercellProxy.Networking.Sessions.Anonymous;

namespace SupercellProxy.Networking.Client;

/// <summary>Creates independent protocol connections using shared DI infrastructure.</summary>
public sealed class ProtocolClientFactory(
    IHttpClientFactory webClients,
    TimeProvider timeProvider,
    ILoggerFactory loggerFactory,
    IServerPublicKeySource serverKeys,
    IAnonymousAccountPool? anonymousAccounts = null
)
{
    private const string ClientWebClientName = "ScClient";

    /// <summary>Creates an online client; the caller owns its lifetime.</summary>
    public ProtocolClient Create(ClientConfiguration configuration)
    {
        return new(
            configuration,
            webClients.CreateClient(ClientWebClientName),
            timeProvider,
            loggerFactory.CreateLogger<ProtocolClient>(),
            serverKeys,
            anonymousAccounts
        );
    }

    /// <summary>Creates a client over an existing stream; the caller owns its lifetime.</summary>
    public ProtocolClient Create(MessageStream stream)
    {
        return new(stream, webClients.CreateClient(ClientWebClientName), timeProvider, loggerFactory.CreateLogger<ProtocolClient>(), serverKeys);
    }
}
