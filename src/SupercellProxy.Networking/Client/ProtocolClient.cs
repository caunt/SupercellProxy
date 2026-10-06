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

    /// <summary>Creates an online connection using injected infrastructure.</summary>
    public ProtocolClient(
        ClientConfiguration configuration,
        HttpClient webClient,
        TimeProvider timeProvider,
        ILogger<ProtocolClient> logger,
        IServerPublicKeySource serverKeys,
        IAnonymousAccountPool? anonymousAccounts = null
    )
    {
        Configuration = configuration;
        _webClient = webClient;
        _serverKeys = serverKeys;
        _timeProvider = timeProvider;
        _logger = logger;
        _anonymousAccounts = anonymousAccounts;
        _authenticator = new ClientAuthenticator(this, new GameAssetCache(webClient, () => Configuration.AssetDirectory));
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
        _authenticator = new ClientAuthenticator(this, new GameAssetCache(webClient, () => Configuration.AssetDirectory));
    }

    /// <summary>Gets the transport keep-alive cadence, independently of game turns.</summary>
    public static TimeSpan KeepAliveInterval { get; } = TimeSpan.FromSeconds(seconds: 5);

    /// <summary>Gets the authenticated or externally supplied message stream.</summary>
    public MessageStream Stream =>
        _supercellStream ?? throw new InvalidOperationException(message: "The client is not connected.");
    internal bool CanUpdateVersion => Configuration.Protocol is null && _serverKeys is HayDayServerPublicKeySource;

    internal ClientConfiguration Configuration =>
        field
        ?? throw new InvalidOperationException(message: "This client has no online configuration.");

    internal ProtocolConfiguration Protocol => _clientVersion?.ToProtocol() ?? Configuration.Protocol
        ?? throw new InvalidOperationException(message: "The protocol version has not been resolved. Custom key sources require an explicit version.");

    /// <summary>Exclusively reuses a saved anonymous account or creates and saves one, without loading its home.</summary>
    public async Task<ClientLoginResult> ConnectAnonymousAsync(CancellationToken cancellationToken = default)
    {
        return await AuthenticateAsync(anonymous: true, requestOwnHome: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>Connects, authenticates, and configures all asset-dependent message codecs.</summary>
    public async Task<ClientLoginResult> ConnectAsync(CancellationToken cancellationToken = default)
    {
        return await AuthenticateAsync(Configuration.AnonymousAccountId is not null, requestOwnHome: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>
    /// Provides the Load Catalog Async value or operation.
    /// </summary>
    public async Task<DataTableResolver> LoadCatalogAsync(CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        using CancellationTokenSource request = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _lifetime.Token);

        using IDisposable gate = await _authenticationGate.LockAsync(request.Token).ConfigureAwait(continueOnCapturedContext: false);

        return _login is { Resources.Length: > 0 } login
            ? new DataTableResolver(login.Resources)
            : await _authenticator.LoadCatalogAsync(request.Token).ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>Receives and decodes the next message without applying game behavior.</summary>
    public async Task<IMessage> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        MessageStream stream = Stream;

        try
        {
            IMessage message = await stream.ReadMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            if (message is DisconnectedMessage) await CloseFailedStreamAsync(stream).ConfigureAwait(continueOnCapturedContext: false);

            return message;
        }
        catch
        {
            await CloseFailedStreamAsync(stream).ConfigureAwait(continueOnCapturedContext: false);

            throw;
        }
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
                LogLoopsStopped(_logger);
            }
        }
    }

    /// <summary>Sends a public message contract without executing its commands or calculating checksums.</summary>
    public async Task SendAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        MessageStream stream = Stream;

        try { await stream.WriteMessageAsync(message, cancellationToken).ConfigureAwait(continueOnCapturedContext: false); }
        catch
        {
            await CloseFailedStreamAsync(stream).ConfigureAwait(continueOnCapturedContext: false);

            throw;
        }
    }

    internal async Task CloseTransportAsync()
    {
        _login = null;

        try
        {
            _supercellStream?.Dispose();

            if (_networkStream is not null) await _networkStream.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
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

        try { await CloseTransportAsync().ConfigureAwait(continueOnCapturedContext: false); }
        finally
        {
            if (lease is not null) await lease.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
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
            _supercellStream = new MessageStream(_networkStream)
            {
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

    private async Task CloseFailedStreamAsync(MessageStream stream)
    {
        using IDisposable gate = await _authenticationGate.LockAsync().ConfigureAwait(continueOnCapturedContext: false);

        if (ReferenceEquals(stream, _supercellStream)) await DisconnectAsync().ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>Disconnects and releases this client's transport and HTTP client.</summary>
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, value: 1) != 0) return;

        await _lifetime.CancelAsync().ConfigureAwait(continueOnCapturedContext: false);

        using IDisposable gate = await _authenticationGate.LockAsync().ConfigureAwait(continueOnCapturedContext: false);

        try { await DisconnectAsync().ConfigureAwait(continueOnCapturedContext: false); }
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
