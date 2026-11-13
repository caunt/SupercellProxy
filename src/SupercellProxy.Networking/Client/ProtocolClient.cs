using System.Net.Sockets;

using Microsoft.Extensions.Logging;

using Nito.AsyncEx;

using SupercellProxy.Networking.Assets;
using SupercellProxy.Networking.Assets.Tables;
using SupercellProxy.Networking.Cryptography;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.ConnectionControl;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;
using SupercellProxy.Networking.Sessions.Anonymous;

namespace SupercellProxy.Networking.Client;

/// <summary>Authenticates and exchanges protocol messages without executing game actions.</summary>
public sealed partial class ProtocolClient : IAsyncDisposable
{
    private readonly IAnonymousAccountPool? _anonymousAccounts;
    private readonly AsyncLock _authenticationGate = new();
    private readonly ClientAuthenticator _authenticator;
    private readonly CancellationTokenSource _lifetime = new();
    private readonly ILogger<ProtocolClient> _logger;
    private readonly IServerPublicKeySource _serverKeys;
    private readonly TimeProvider _timeProvider;
    private readonly HttpClient _webClient;
    private AnonymousAccountLease? _anonymousLease;
    private HayDayServerKey? _clientVersion;
    private int _disposed;
    private ClientLoginResult? _login;
    private NetworkStream? _networkStream;
    private TcpClient? _socketClient;
    private MessageStream? _supercellStream;
    private ConnectionTraffic? _traffic;

    /// <summary>Initializes a new instance of the <see cref="ProtocolClient"/> class. Creates an online connection using injected infrastructure.</summary>
    /// <param name="configuration">The server address, protocol version, session token source and asset directory the client logs in with.</param>
    /// <param name="webClient">HTTP client for asset downloads and version lookups; the protocol client disposes it when it is disposed.</param>
    /// <param name="timeProvider">Clock for the keep-alive timer and traffic averaging.</param>
    /// <param name="logger">Receives the client's debug logging.</param>
    /// <param name="serverKeys">Supplies the server's public key; the built-in Hay Day key source also lets the client pick the latest client version when <see cref="ClientConfiguration.Protocol"/> is <see langword="null"/>.</param>
    /// <param name="anonymousAccounts">The pool of saved anonymous accounts that anonymous logins lease from, or <see langword="null"/> to log in with a fresh unsaved anonymous account; a configured <see cref="ClientConfiguration.AnonymousAccountId"/> requires a pool.</param>
    /// <remarks>Nothing connects until <see cref="ConnectAsync"/>, <see cref="ConnectAnonymousAsync"/> or <see cref="RunAsync"/> is called.</remarks>
    public ProtocolClient(ClientConfiguration configuration,
        HttpClient webClient,
        TimeProvider timeProvider,
        ILogger<ProtocolClient> logger,
        IServerPublicKeySource serverKeys,
        IAnonymousAccountPool? anonymousAccounts = null)
    {
        // Keep the online settings and the services used to reach the server.
        Configuration = configuration;
        _webClient = webClient;
        _serverKeys = serverKeys;

        // Keep the clock, logging and the pool that anonymous logins lease accounts from.
        _timeProvider = timeProvider;
        _logger = logger;
        _anonymousAccounts = anonymousAccounts;

        // The asset directory is read on each lookup, not captured here.
        _authenticator = new ClientAuthenticator(this, new GameAssetCache(webClient, () => Configuration.AssetDirectory));
    }

    /// <summary>Initializes a new instance of the <see cref="ProtocolClient"/> class. Creates a protocol client over an existing stream, including offline streams.</summary>
    /// <param name="stream">The message stream to read and write, such as a replayed capture; it gets <paramref name="serverKeys"/> as its key source when it has none.</param>
    /// <param name="webClient">HTTP client for asset downloads; the protocol client disposes it when it is disposed.</param>
    /// <param name="timeProvider">Clock for the keep-alive timer and traffic averaging.</param>
    /// <param name="logger">Receives the client's debug logging.</param>
    /// <param name="serverKeys">Supplies the server's public key for the encrypted handshake.</param>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <see langword="null"/>.</exception>
    /// <remarks>A client built this way has no <see cref="ClientConfiguration"/>, so it cannot connect or log in; it only sends and receives on the given stream.</remarks>
    public ProtocolClient(MessageStream stream,
        HttpClient webClient,
        TimeProvider timeProvider,
        ILogger<ProtocolClient> logger,
        IServerPublicKeySource serverKeys)
    {
        ArgumentNullException.ThrowIfNull(stream);

        // Adopt the given stream, lending it this client's key source only when it has none of its own.
        _supercellStream = stream;
        stream.ServerKeySource ??= serverKeys;

        // Keep the services for downloads, the handshake key, timing and logging.
        _webClient = webClient;
        _serverKeys = serverKeys;
        _timeProvider = timeProvider;
        _logger = logger;

        // A stream-only client has no configuration, so the asset directory is looked up only when assets are needed.
        _authenticator = new ClientAuthenticator(this, new GameAssetCache(webClient, () => Configuration.AssetDirectory));
    }

    /// <summary>Gets the transport keep-alive cadence, independently of game turns.</summary>
    /// <value>Five seconds; <see cref="RunAsync"/> sends one keep-alive message per interval.</value>
    public static TimeSpan KeepAliveInterval { get; } = TimeSpan.FromSeconds(seconds: 5);

    /// <summary>Gets the authenticated or externally supplied message stream.</summary>
    /// <value>The stream given to the stream constructor, or the one opened by the current connection; it is cleared when the client disconnects.</value>
    /// <exception cref="InvalidOperationException">The client has no stream: it has not connected yet, or it has disconnected.</exception>
    public MessageStream Stream =>
        _supercellStream ?? throw new InvalidOperationException(message: "The client is not connected.");

    /// <summary>Gets traffic for the current or most recently closed socket, reset when a new socket connects.</summary>
    /// <value>Byte counts and connection time of the last socket this client opened, or <see langword="null"/> before it has opened one, including for a client built over an existing stream.</value>
    public ConnectionTrafficSnapshot? Traffic => _traffic?.Read();
    internal bool CanUpdateVersion => Configuration.Protocol is null && _serverKeys is HayDayServerPublicKeySource;

    internal ClientConfiguration Configuration =>
        field
        ?? throw new InvalidOperationException(message: "This client has no online configuration.");

    internal ProtocolConfiguration Protocol => _clientVersion?.ToProtocol() ?? Configuration.Protocol
        ?? throw new InvalidOperationException(message: "The protocol version has not been resolved. Custom key sources require an explicit version.");

    /// <summary>Exclusively reuses a saved anonymous account or creates and saves one, without loading its home.</summary>
    /// <param name="cancellationToken">Cancels the asset bootstrap, the account lease and the login.</param>
    /// <returns>The anonymous login confirmation, whose account id and pass token have been saved when the client has an account pool; an existing login of this client is returned unchanged.</returns>
    public async Task<ClientLoginResult> ConnectAnonymousAsync(CancellationToken cancellationToken = default)
    {
        return await AuthenticateAsync(anonymous: true, requestOwnHome: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>Connects, authenticates, and configures all asset-dependent message codecs.</summary>
    /// <param name="cancellationToken">Cancels the connection and login; a cancelled login disconnects the half-open session.</param>
    /// <returns>The login confirmation and the asset fingerprint and files it was made with; when the client is already logged in, the existing result is returned without reconnecting.</returns>
    /// <remarks>The client logs in with the configured session token, or as the configured saved anonymous account when <see cref="ClientConfiguration.AnonymousAccountId"/> is set, and asks the server for the player's own home.</remarks>
    public async Task<ClientLoginResult> ConnectAsync(CancellationToken cancellationToken = default)
    {
        return await AuthenticateAsync(Configuration.AnonymousAccountId is not null, requestOwnHome: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>
    /// Gets the game data tables. After a login this reuses the session's bundle; before one, it runs a throwaway login that only
    /// provokes the asset manifest, then downloads the files without logging in.
    /// </summary>
    /// <param name="cancellationToken">Stops the download or handshake; disposing the client stops it too.</param>
    /// <returns>A resolver over the CSV data tables of the asset fingerprint in use; the shared resolver of the current login when it carried assets, otherwise one built from the freshly downloaded files.</returns>
    /// <exception cref="ObjectDisposedException">The client was disposed.</exception>
    public async Task<DataTableResolver> LoadCatalogAsync(CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        using CancellationTokenSource request = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _lifetime.Token);

        using IDisposable gate = await _authenticationGate.LockAsync(request.Token).ConfigureAwait(continueOnCapturedContext: false);

        return _login is { Resources.Length: > 0 } login
            ? DataTableResolver.Shared(login.Fingerprint, login.Resources)
            : await _authenticator.LoadCatalogAsync(request.Token).ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>Receives and decodes the next message without applying game behavior.</summary>
    /// <param name="cancellationToken">Cancels waiting for the next frame; a cancelled or failed read disconnects the client before the error is rethrown.</param>
    /// <returns>The next clientbound message, decoded with the session's data tables; a <see cref="TitanDisconnectedMessage"/> is still returned, but the client has already disconnected when it is.</returns>
    public async Task<IMessage> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        MessageStream stream = Stream;

        try
        {
            IMessage message = await stream.ReadMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            if (message is TitanDisconnectedMessage)
                await CloseFailedAsync(stream).ConfigureAwait(continueOnCapturedContext: false);

            return message;
        }
        catch
        {
            await CloseFailedAsync(stream).ConfigureAwait(continueOnCapturedContext: false);

            throw;
        }
    }

    /// <summary>Runs an authenticated message consumer with protocol keep-alives until disconnected or cancelled.</summary>
    /// <param name="onMessage">Called with each received message, one at a time and in arrival order; the next message is not read until the returned task completes, and an exception from it ends the run.</param>
    /// <param name="cancellationToken">Stops the login, the receive loop and the keep-alive timer; cancelling ends the run with an <see cref="OperationCanceledException"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="onMessage"/> is <see langword="null"/>.</exception>
    /// <remarks>The method first calls <see cref="ConnectAsync"/>, then sends a keep-alive every <see cref="KeepAliveInterval"/> while it receives; whichever loop fails or stops first stops the other.</remarks>
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
                LogLoopsStopped(_logger);
            }
        }
    }

    /// <summary>Sends a public message contract without executing its commands or calculating checksums.</summary>
    /// <param name="message">The message to encode, encrypt and write to the current stream; any turn checksum it carries is sent as given.</param>
    /// <param name="cancellationToken">Cancels the write; a cancelled or failed write disconnects the client before the error is rethrown.</param>
    public async Task SendAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        MessageStream stream = Stream;

        try
        {
            await stream.WriteAsync(message, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
        catch
        {
            await CloseFailedAsync(stream).ConfigureAwait(continueOnCapturedContext: false);

            throw;
        }
    }

    internal async Task CloseTransportAsync()
    {
        _login = null;

        try
        {
            _supercellStream?.Dispose();

            if (_networkStream is not null)
                await _networkStream.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            _socketClient?.Dispose();

            _supercellStream = null;
            _networkStream = null;
            _socketClient = null;
        }
    }

    internal async Task DisconnectAsync()
    {
        AnonymousAccountLease? lease = Interlocked.Exchange(ref _anonymousLease, value: null);

        try
        {
            await CloseTransportAsync().ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            if (lease is not null)
                await lease.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    internal async Task<MessageStream> GetStreamAsync(CancellationToken cancellationToken = default)
    {
        if (_supercellStream is null)
        {
            if (CanUpdateVersion && _serverKeys is HayDayServerPublicKeySource source)
                _clientVersion = await source.GetLatestAsync(refresh: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            ProtocolConfiguration protocol = Protocol;

            _socketClient = new TcpClient();

            await _socketClient
                .ConnectAsync(Configuration.UpstreamHost, Configuration.UpstreamPort, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            _networkStream = _socketClient.GetStream();
            _traffic = new ConnectionTraffic(_timeProvider);

            _supercellStream = new MessageStream(_networkStream)
            {
                Traffic = _traffic,
                ServerKeySource = (IServerPublicKeySource?)_clientVersion ?? _serverKeys,
                ObserveMessage = ObserveMessageAsync,
                OutboundMessageVersion = protocol.MessageVersion,
            };
        }

        return _supercellStream;
    }

    internal async Task RefreshVersionAsync(CancellationToken cancellationToken)
    {
        if (_serverKeys is HayDayServerPublicKeySource source)
            _clientVersion = await source.GetLatestAsync(refresh: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    [LoggerMessage(Level = LogLevel.Debug, Message = "Authenticating protocol connection")]
    private static partial void LogAuthenticating(ILogger logger);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Client heartbeat and receive loop stopped.")]
    private static partial void LogLoopsStopped(ILogger logger);

    private async Task CloseFailedAsync(MessageStream stream)
    {
        using IDisposable gate = await _authenticationGate.LockAsync().ConfigureAwait(continueOnCapturedContext: false);

        if (ReferenceEquals(stream, _supercellStream))
            await DisconnectAsync().ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>Disconnects and releases this client's transport and HTTP client.</summary>
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, value: 1) != 0)
            return;

        await _lifetime.CancelAsync().ConfigureAwait(continueOnCapturedContext: false);

        using IDisposable gate = await _authenticationGate.LockAsync().ConfigureAwait(continueOnCapturedContext: false);

        try
        {
            await DisconnectAsync().ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            _webClient.Dispose();
            _lifetime.Dispose();
        }
    }

    private async Task KeepAliveAsync(CancellationToken cancellationToken)
    {
        using PeriodicTimer timer = new(KeepAliveInterval, _timeProvider);

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
