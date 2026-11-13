using System.Globalization;
using System.Net.Sockets;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Cryptography;
using SupercellProxy.Networking.Events;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Turns;
using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Sessions.Anonymous;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Proxy;

/// <summary>Owns the downstream and upstream connections and forwards their protocol traffic.</summary>
public sealed partial class ProxyConnection : IAsyncDisposable
{
    private readonly ProxyHandshake _handshake;
    private readonly ProxyHomeVisitor _homeVisitor;
    private readonly ILogger _logger;

    private MessageChannel? _serverStream;
    private bool _started;

    private ProxyConnection(TcpClient socketClient,
        ProxyCaptureWriter trafficCapture,
        Func<bool, CancellationToken, Task<SessionTokenData>>? sessionTokenProvider,
        ILogger? logger,
        AnonymousAccountLease? anonymousAccount,
        CancellationToken cancellationToken)
    {
        // Keep the accepted game-client socket and the helpers that act on its traffic.
        SocketClient = socketClient;
        _logger = logger ?? NullLogger.Instance;
        _homeVisitor = new ProxyHomeVisitor(this);

        // The upstream socket connects later, while the already connected game client is wrapped as a message channel now.
        SocketUpstream = new TcpClient();
        ClientStream = new MessageChannel(socketClient.GetStream());

        // Events and cancellation belong to this connection, while the caller's token can still end it.
        EventBus = new EventBus();
        CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        // Record where traffic is captured, who the peer is, and how the upstream login gets its credentials.
        TrafficCapture = trafficCapture;
        RemoteEndPoint = socketClient.GetRemoteEndPoint();
        _handshake = new ProxyHandshake(sessionTokenProvider, anonymousAccount);
    }

    /// <summary>
    /// Gets the source that ends the whole connection when cancelled: both forwarding loops stop and both sockets are dropped. Linked to the token the
    /// connection was opened with.
    /// </summary>
    /// <value>A linked source owned by the connection; it is cancelled automatically when either forwarding loop ends and disposed with the connection.</value>
    public CancellationTokenSource CancellationTokenSource { get; }

    /// <summary>
    /// Gets the encrypted message channel to the game client on the local side.
    /// </summary>
    /// <value>Stream over the accepted client socket, created with the connection; encryption is set up on it when the server hello is forwarded.</value>
    public MessageChannel ClientStream { get; }

    /// <summary>
    /// Gets a task that finishes when both forwarding loops have stopped; already complete before <see cref="RunAsync"/> starts them.
    /// </summary>
    /// <value>Replaced by the forwarding task when <see cref="RunAsync"/> starts; it ends once one side disconnects and the other loop has been stopped.</value>
    public Task CompletionTask { get; private set; } = Task.CompletedTask;

    /// <summary>
    /// Gets the bus that raises an event for every message this connection receives (cancellable) and forwards, so callers can watch or filter traffic.
    /// </summary>
    /// <value>Bus that publishes a <c>MessageReceivedEvent</c> before forwarding, which a handler can cancel to drop the message, and a <c>MessageSentEvent</c> after it is written.</value>
    public EventBus EventBus { get; }

    /// <summary>
    /// Gets the address and port of the game client, used to name the connection in logs and its capture folder.
    /// </summary>
    /// <value>Text form of the client socket's remote endpoint, such as <c>192.168.1.5:51234</c>, read once when the connection is created.</value>
    public string RemoteEndPoint { get; }

    /// <summary>
    /// Gets the encrypted message channel to the real game server.
    /// </summary>
    /// <value>Stream over the upstream socket, created once the connection to the server succeeds and set up with the server key source and command tables.</value>
    /// <exception cref="InvalidOperationException">The upstream socket is not connected yet.</exception>
    public MessageChannel ServerStream =>
        _serverStream ?? throw new InvalidOperationException(message: "The upstream stream is unavailable.");

    /// <summary>
    /// Gets the socket of the game client that connected to the proxy.
    /// </summary>
    /// <value>The accepted client socket the connection was opened with; disposing the connection disposes it too.</value>
    [JsonPropertyName("TcpClient")]
    public TcpClient SocketClient { get; }

    /// <summary>
    /// Gets the socket the proxy opened to the real game server for this client.
    /// </summary>
    /// <value>A socket created with the connection and connected to the upstream host before the connection is returned; disposed together with the connection.</value>
    [JsonPropertyName("TcpUpstream")]
    public TcpClient SocketUpstream { get; }

    /// <summary>
    /// Gets the writer that records this connection's frames, both as received and as written after any rewriting.
    /// </summary>
    /// <value>The capture writer passed when the connection was opened; it also decides whether original frame bytes are preserved while forwarding.</value>
    public ProxyCaptureWriter TrafficCapture { get; }

    /// <summary>
    /// Opens the upstream socket for an accepted game client and prepares both encrypted streams; traffic flows once
    /// <see cref="RunAsync"/> is called.
    /// </summary>
    /// <param name="socketClient">The accepted game client.</param>
    /// <param name="upstreamHost">Game server host to connect to.</param>
    /// <param name="upstreamPort">Game server port.</param>
    /// <param name="trafficCapture">Writer for this connection's capture.</param>
    /// <param name="sessionTokenProvider">
    /// When set, the client's login is rewritten to log in with this Supercell ID session token instead of the client's own
    /// credentials; called again with <see langword="true"/> to refresh a token the server rejects.
    /// </param>
    /// <param name="serverKeys">Source of the real server's public key.</param>
    /// <param name="commandDataResolver">Game tables used to decode commands in both directions.</param>
    /// <param name="logger">Receives login and decoding warnings.</param>
    /// <param name="cancellationToken">Ends the connection, now or later.</param>
    /// <returns>A connection whose upstream socket is connected and whose two streams are ready; the caller owns it and must dispose it.</returns>
    public static Task<ProxyConnection> ConnectAsync(TcpClient socketClient,
        string upstreamHost,
        int upstreamPort,
        ProxyCaptureWriter trafficCapture,
        Func<bool, CancellationToken, Task<SessionTokenData>>? sessionTokenProvider = null,
        IServerPublicKeySource? serverKeys = null,
        ICommandDataResolver? commandDataResolver = null,
        ILogger? logger = null,
        CancellationToken cancellationToken = default)
    {
        return ConnectAsync(socketClient,
            upstreamHost,
            upstreamPort,
            trafficCapture,
            sessionTokenProvider,
            serverKeys,
            commandDataResolver,
            logger,
            anonymousAccount: null,
            cancellationToken);
    }

    /// <summary>Opens a proxy connection using a host-owned anonymous account lease.</summary>
    /// <param name="socketClient">The accepted game client.</param>
    /// <param name="upstreamHost">Game server host name or address to connect to.</param>
    /// <param name="upstreamPort">Game server TCP port.</param>
    /// <param name="trafficCapture">Writer for this connection's capture.</param>
    /// <param name="sessionTokenProvider">When set and no anonymous lease is given, rewrites the client's login to this Supercell ID session token; called with <see langword="true"/> to refresh a rejected token.</param>
    /// <param name="serverKeys">Source of the real server's public key, or <see langword="null"/> when the upstream stream needs none.</param>
    /// <param name="commandDataResolver">Game tables used to decode commands in both directions, or <see langword="null"/> to decode without them.</param>
    /// <param name="logger">Receives login and decoding warnings; <see langword="null"/> discards them.</param>
    /// <param name="anonymousAccount">Lease whose saved account id and pass token replace the client's login and whose token is saved again when the server accepts it; it takes precedence over <paramref name="sessionTokenProvider"/>, and <see langword="null"/> leaves the login alone.</param>
    /// <param name="cancellationToken">Ends the connection, now or later, including while the upstream socket is still connecting.</param>
    /// <returns>A connection whose upstream socket is connected and whose two streams are ready; if connecting fails, the partly built connection is disposed and the error rethrown.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="socketClient"/> is <see langword="null"/>.</exception>
    public static async Task<ProxyConnection> ConnectAsync(TcpClient socketClient,
        string upstreamHost,
        int upstreamPort,
        ProxyCaptureWriter trafficCapture,
        Func<bool, CancellationToken, Task<SessionTokenData>>? sessionTokenProvider,
        IServerPublicKeySource? serverKeys,
        ICommandDataResolver? commandDataResolver,
        ILogger? logger,
        AnonymousAccountLease? anonymousAccount,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(socketClient);

        ProxyConnection client = new(socketClient, trafficCapture, sessionTokenProvider, logger, anonymousAccount, cancellationToken);

        try
        {
            await client.SocketUpstream
                .ConnectAsync(upstreamHost, upstreamPort, client.CancellationTokenSource.Token)
                .ConfigureAwait(continueOnCapturedContext: false);

            client.ClientStream.CommandDataResolver = commandDataResolver;

            client._serverStream = new MessageChannel(client.SocketUpstream.GetStream())
            {
                ServerKeySource = serverKeys,
                CommandDataResolver = commandDataResolver,
            };

            return client;
        }
        catch
        {
            await ((IAsyncDisposable)client).DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);

            throw;
        }
    }

    /// <summary>
    /// Forwards traffic both ways until either side disconnects, handling the handshake (encryption setup and login rewriting) on
    /// the way. Can run only once per connection.
    /// </summary>
    /// <param name="cancellationToken">Stops waiting; cancel <see cref="CancellationTokenSource"/> to stop the forwarding itself.</param>
    /// <exception cref="InvalidOperationException">The connection was already started.</exception>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        if (_started)
            throw new InvalidOperationException(message: "The proxy client is already running.");

        _started = true;

        await EventBus.SubscribeAsync<MessageReceivedEvent>(OnMessageReceivedAsync, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        await EventBus.SubscribeAsync<MessageSentEvent>(OnMessageSentAsync, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        try
        {
            // Handshake handlers must be subscribed before either pump can observe ServerHello or Login.
            CompletionTask = RunPumpsAsync(CancellationTokenSource.Token);

            await CompletionTask.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            await EventBus.UnsubscribeAsync<MessageReceivedEvent>(OnMessageReceivedAsync, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await EventBus.UnsubscribeAsync<MessageSentEvent>(OnMessageSentAsync, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    /// <summary>
    /// Names the connection by the game client's address and port.
    /// </summary>
    /// <returns>The value of <see cref="RemoteEndPoint"/>.</returns>
    public override string ToString()
    {
        return RemoteEndPoint;
    }

    /// <summary>
    /// Makes the proxy visit another farm on the client's behalf and returns that farm's data. The client's own end-of-turn
    /// messages are held back while the visit settles.
    /// </summary>
    /// <param name="target">Home id of the farm to visit.</param>
    /// <param name="cancellationToken">Stops waiting for the farm data.</param>
    /// <returns>The farm data message the server sent back for <paramref name="target"/>, returned after the client's follow-up turns have settled or gone quiet for 3 seconds.</returns>
    /// <exception cref="TimeoutException">The farm data did not arrive within 15 seconds.</exception>
    public ValueTask<OtherHomeDataMessage> VisitHomeAsync(LongId target, CancellationToken cancellationToken = default)
    {
        return _homeVisitor.VisitHomeAsync(target, cancellationToken);
    }

    internal async Task WriteAsync(IMessage message, MessageDirection direction, CancellationToken cancellationToken = default)
    {
        MessageChannel source = direction is MessageDirection.Clientbound ? ServerStream : ClientStream;
        MessageChannel destination = direction is MessageDirection.Clientbound ? ClientStream : ServerStream;
        MessageContainer outgoingContainer = MessageContainer.Create(message, destination.OutboundMessageVersion, destination.GameVersion);

        await TrafficCapture.SaveAsync(stage: "outgoing", direction, outgoingContainer, MessageRegistry.GetCaptureName(message), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        await destination.WriteContainerAsync(outgoingContainer, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        await EventBus.PublishAsync(new MessageSentEvent(message, direction, source, destination), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "Upstream login rejected: {Reason}. The original response was forwarded unchanged.")]
    private static partial void LogLoginRejected(ILogger logger, string reason);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Forwarding packet {Id}/{Version} without optional decoding: {Reason}")]
    private static partial void LogUndecoded(ILogger logger, ushort id, ushort version, string reason);

    private bool CanForwardUndecoded(ushort id, Exception exception)
    {
        return TrafficCapture.PreserveForwardedFrames
            && exception is InvalidDataException or NotSupportedException or EndOfStreamException
            && id != MessageRegistry.GetId<ClientHelloMessage>(gameVersion: null)
            && id != MessageRegistry.GetId<ServerHelloMessage>(gameVersion: null)
            && id != MessageRegistry.GetId<LoginMessage>(gameVersion: null)
            && id != MessageRegistry.GetId<LoginOkMessage>(gameVersion: null)
            && id != MessageRegistry.GetId<LoginFailedMessage>(gameVersion: null);
    }

    /// <summary>
    /// Executes the <c language="csharp">DisposeAsync</c> operation.
    /// </summary>
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        // Close both message channels and their sockets so the forwarding loops stop.
        ClientStream.Dispose();
        _serverStream?.Dispose();
        SocketClient.Dispose();
        SocketUpstream.Dispose();

        // Unlink from the caller's token so the connection no longer listens for its cancellation.
        CancellationTokenSource.Dispose();

        await CompletionTask.ConfigureAwait(continueOnCapturedContext: false);
    }

    private async Task OnMessageReceivedAsync(MessageReceivedEvent @event, CancellationToken cancellationToken = default)
    {
        await _handshake.OnMessageReceivedAsync(@event, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        switch (@event.Message)
        {
            case VisitHomeTargetMessage visitHomeTargetMessage when visitHomeTargetMessage.Target == LongId.Empty:
                {
                    @event.IsCancelled = true;

                    break;
                }
            case EndClientTurnMessage when _homeVisitor.SuppressEndClientTurns:
                {
                    @event.IsCancelled = true;

                    break;
                }
            default:
                break;
        }

        if (@event.IsCancelled)
        {
            Console.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"[{DateTime.Now:T}] CANCELLED {@event.Direction} {(@event.Message is PassthroughMessage ? @event.Message : string.Empty)}"));
        }
    }

    private async Task OnMessageSentAsync(MessageSentEvent @event, CancellationToken cancellationToken = default)
    {
        if (@event.Message is LoginFailedMessage failure && @event.Direction is MessageDirection.Clientbound)
            LogLoginRejected(_logger, new LoginException(failure).Message);

        await _handshake.OnMessageSentAsync(@event, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        // The structured warning above is intentionally concise; the record's default rendering
        // includes the complete asset catalog carried by OutdatedContent.
        if (@event.Message is LoginFailedMessage)
            return;

        if (@event.Message is EndClientTurnMessage endClientTurnMessage)
        {
            Console.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"[{DateTime.Now:T}] {nameof(EndClientTurnMessage)} {{ Checksum = {endClientTurnMessage.Checksum}, SubTick = {endClientTurnMessage.SubTick}, SubChecksums = [{string.Join(separator: ',', endClientTurnMessage.SubChecksums.ToArray())}], Commands = [{string.Join(separator: ',', endClientTurnMessage.Commands.ToArray().Select(static command => command.Contract.Name))}] }}"));
        }
        else if (@event.Message is PassthroughMessage)
        {
            Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"[{DateTime.Now:T}] {@event.Direction} {@event.Message}"));
        }
        else
        {
            Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"[{DateTime.Now:T}] {@event.Message}"));
        }
    }

    private async Task PumpAsync(MessageDirection direction, CancellationToken cancellationToken)
    {
        MessageChannel source = direction is MessageDirection.Clientbound ? ServerStream : ClientStream;
        MessageChannel destination = direction is MessageDirection.Clientbound ? ClientStream : ServerStream;

        while (!cancellationToken.IsCancellationRequested)
        {
            (IMessage message, MessageContainer original) = await ReadForwardMessageAsync(source, direction, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            MessageReceivedEvent @event = new(message, direction, source, destination);

            await EventBus.PublishAsync(@event, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            if (@event.IsCancelled)
                continue;

            await WriteForwardAsync(message, original, destination, direction, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await EventBus.PublishAsync(new MessageSentEvent(message, direction, source, destination), cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private async Task<DecodedMessageFrame> ReadForwardMessageAsync(MessageChannel source, MessageDirection direction, CancellationToken cancellationToken)
    {
        try
        {
            MessageContainer container = await source.ReadContainerAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (direction is MessageDirection.Serverbound && container.Id == MessageRegistry.GetId<LoginMessage>(gameVersion: null))
                ServerStream.OutboundMessageVersion = container.Version;

            await TrafficCapture.SaveAsync(stage: "incoming", direction, container, messageName: "frame", cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            try
            {
                return new DecodedMessageFrame(source.ResolveMessage(container), container);
            }
            catch (Exception exception) when (CanForwardUndecoded(container.Id, exception))
            {
                LogUndecoded(_logger, container.Id, container.Version, exception.Message);

                return new DecodedMessageFrame(new PassthroughMessage { Id = container.Id, Version = container.Version, Data = container.Payload.ToArray() }, container);
            }
        }
        catch (StreamClosedException exception)
        {
            string side = direction is MessageDirection.Clientbound ? "server" : "client";

            throw new StreamClosedException($"{side} closed the connection", exception);
        }
    }

    private async Task RunPumpsAsync(CancellationToken cancellationToken)
    {
        Task serverboundPumpTask = PumpAsync(MessageDirection.Serverbound, cancellationToken);
        Task clientboundPumpTask = PumpAsync(MessageDirection.Clientbound, cancellationToken);
        string remoteEndPoint = SocketClient.GetRemoteEndPoint();

        Task completedTask = await Task.WhenAny(serverboundPumpTask, clientboundPumpTask)
            .ConfigureAwait(continueOnCapturedContext: false);

        try
        {
            await completedTask.ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (MacVerificationException exception) when (exception.IsPublicKeyBox)
        {
            Console.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"[{DateTime.Now:T}] {remoteEndPoint} closed: messages were encrypted likely with an incorrect public key"));
        }
        catch (StreamClosedException exception)
        {
            Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"[{DateTime.Now:T}] {remoteEndPoint} closed: {exception.Message}"));
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine($"{remoteEndPoint} proxy connection stopped.");
        }
        catch (Exception exception) when (ProxyFailureClassifier.IsRecoverable(exception))
        {
            Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"[{DateTime.Now:T}] {remoteEndPoint} closed: {exception}"));
        }
        finally
        {
            await StopPumpsAsync(completedTask, serverboundPumpTask, clientboundPumpTask)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private async Task StopPumpsAsync(Task completedTask, Task serverboundPumpTask, Task clientboundPumpTask)
    {
        await CancellationTokenSource.CancelAsync().ConfigureAwait(continueOnCapturedContext: false);

        try
        {
            await (
                completedTask == serverboundPumpTask ? clientboundPumpTask : serverboundPumpTask
            ).ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine($"{RemoteEndPoint} proxy pumps stopped.");
        }
    }

    private async Task WriteForwardAsync(IMessage message,
        MessageContainer original,
        MessageChannel destination,
        MessageDirection direction,
        CancellationToken cancellationToken)
    {
        // A rejection before Login can carry the asset fingerprint and download locations needed by
        // an outdated client. Preserve that server response even though device identity has not yet
        // been established; Login remains the only message the proxy intentionally replaces.
        bool preserveOriginal = (direction is MessageDirection.Clientbound && message is LoginFailedMessage)
            || (TrafficCapture.PreserveForwardedFrames && message is not LoginMessage);

        MessageContainer container = preserveOriginal ? original : MessageContainer.Create(message, original.Version, destination.GameVersion);

        await TrafficCapture.SaveAsync(stage: "outgoing", direction, container, MessageRegistry.GetCaptureName(message), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        try
        {
            await destination.WriteContainerAsync(container, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (StreamClosedException exception)
        {
            string side = direction is MessageDirection.Clientbound ? "client" : "server";

            throw new StreamClosedException($"{side} closed the connection", exception);
        }
    }
}
