using System.Net.Sockets;

using Microsoft.Extensions.Logging;

using SupercellProxy.Networking.Assets;
using SupercellProxy.Networking.Assets.Tables;
using SupercellProxy.Networking.Cryptography;
using SupercellProxy.Networking.Hosting;
using SupercellProxy.Networking.Protocol.ConnectionControl;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Client;

/// <summary>Authenticates and exchanges protocol messages without executing game actions.</summary>
public sealed class ProtocolClient : IAsyncDisposable
{
    private readonly ClientAuthenticator _authenticator;
    private readonly ILogger<ProtocolClient> _logger;
    private readonly IServerPublicKeySource _serverKeys;
    private readonly TimeProvider _timeProvider;
    private readonly HttpClient _webClient;
    private ClientLoginResult? _login;
    private NetworkStream? _networkStream;
    private TcpClient? _socketClient;
    private MessageStream? _supercellStream;

    /// <summary>Creates an online connection using injected infrastructure.</summary>
    public ProtocolClient(
        ClientConfiguration configuration,
        HttpClient webClient,
        TimeProvider timeProvider,
        ILogger<ProtocolClient> logger,
        IServerPublicKeySource serverKeys
    )
    {
        Configuration = configuration;
        _webClient = webClient;
        _serverKeys = serverKeys;
        _timeProvider = timeProvider;
        _logger = logger;
        _authenticator = new ClientAuthenticator(this, new SessionTokenRefresher(webClient, timeProvider), new GameAssetCache(webClient, () => Configuration.AssetDirectory));
    }

    /// <summary>Creates a protocol client over an existing stream, including offline streams.</summary>
    public ProtocolClient(
        MessageStream stream,
        HttpClient webClient,
        TimeProvider timeProvider,
        ILogger<ProtocolClient> logger,
        IServerPublicKeySource serverKeys
    )
    {
        ArgumentNullException.ThrowIfNull(stream);
        _supercellStream = stream;
        stream.ServerKeySource ??= serverKeys;
        _webClient = webClient;
        _serverKeys = serverKeys;
        _timeProvider = timeProvider;
        _logger = logger;
        _authenticator = new ClientAuthenticator(this, new SessionTokenRefresher(webClient, timeProvider), new GameAssetCache(webClient, () => Configuration.AssetDirectory));
    }

    /// <summary>Gets the authenticated or externally supplied message stream.</summary>
    public MessageStream Stream =>
        _supercellStream ?? throw new InvalidOperationException(message: "The client is not connected.");
    internal ClientConfiguration Configuration =>
        field
        ?? throw new InvalidOperationException(message: "This client has no online configuration.");

    /// <summary>Connects, authenticates, and configures all asset-dependent message codecs.</summary>
    public async Task<ClientLoginResult> ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (_login is not null)
            return _login;

        _logger.Log(
            LogLevel.Debug,
            new EventId(id: 1, name: "Connecting"),
            state: "Authenticating protocol connection",
            exception: null,
            static (message, unusedParameter1) => message
        );
        _login = await _authenticator.LoginAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (_login.Resources.Length > 0)
            Stream.CommandDataResolver = new DataTableResolver(_login.Resources);

        return _login;
    }

    /// <summary>
    /// Provides the Load Catalog Async value or operation.
    /// </summary>
    public Task<DataTableResolver> LoadCatalogAsync(CancellationToken cancellationToken)
    {
        return _authenticator.LoadCatalogAsync(cancellationToken);
    }

    /// <summary>Receives and decodes the next message without applying game behavior.</summary>
    public Task<IMessage> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        return Stream.ReadMessageAsync(cancellationToken);
    }

    /// <summary>Runs an authenticated message consumer with protocol keep-alives until disconnected or cancelled.</summary>
    public async Task RunAsync(Func<IMessage, CancellationToken, Task> onMessage, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(onMessage);
        _login = await ConnectAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        using CancellationTokenSource lifetime = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        Task heartbeat = KeepAliveAsync(lifetime.Token);
        Task receive = ReceiveLoopAsync(onMessage, lifetime.Token);

        try
        {
            Task completed = await Task.WhenAny(heartbeat, receive).ConfigureAwait(continueOnCapturedContext: false);
            await completed.ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            await lifetime.CancelAsync().ConfigureAwait(continueOnCapturedContext: false);

            try
            {
                await Task.WhenAll(heartbeat, receive).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (OperationCanceledException) when (lifetime.IsCancellationRequested)
            {
                ConnectionLog.Debug(_logger, message: "Client heartbeat and receive loop stopped.");
            }
        }
    }

    /// <summary>Sends a public message contract without executing its commands or calculating checksums.</summary>
    public Task SendAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        return Stream.WriteMessageAsync(message, cancellationToken);
    }

    internal async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        _login = null;
        _supercellStream?.Dispose();
        _supercellStream = null;

        if (_networkStream is not null)
        {
            await _networkStream.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
            _networkStream = null;
        }

        _socketClient?.Dispose();
        _socketClient = null;
    }

    internal async Task<MessageStream> GetStreamAsync(CancellationToken cancellationToken = default)
    {
        if (_supercellStream is null)
        {
            _socketClient = new TcpClient();
            await _socketClient
                .ConnectAsync(Configuration.UpstreamHost, Configuration.UpstreamPort, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            _networkStream = _socketClient.GetStream();
            _supercellStream = new MessageStream(_networkStream) { ServerKeySource = _serverKeys };
        }

        return _supercellStream;
    }

    /// <summary>Disconnects and releases this client's transport and HTTP client.</summary>
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await DisconnectAsync().ConfigureAwait(continueOnCapturedContext: false);
        _webClient.Dispose();
    }

    private async Task KeepAliveAsync(CancellationToken cancellationToken)
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(seconds: 5), _timeProvider);

        while (await timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
            await SendAsync(new KeepAliveMessage(), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private async Task ReceiveLoopAsync(Func<IMessage, CancellationToken, Task> onMessage, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await onMessage(await ReceiveAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false), cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
