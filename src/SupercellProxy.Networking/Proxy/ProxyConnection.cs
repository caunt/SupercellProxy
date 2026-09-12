using System.Globalization;
using System.Net.Sockets;

using Microsoft.Extensions.Logging;

using SupercellProxy.Networking.Cryptography;
using SupercellProxy.Networking.Events;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Turns;
using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Proxy;

/// <summary>Owns the downstream and upstream connections and forwards their protocol traffic.</summary>
public sealed class ProxyConnection : IAsyncDisposable
{
    private readonly ProxyHandshake _handshake;
    private readonly ProxyHomeVisitor _homeVisitor;
    private MessageStream? _serverStream;
    private bool _started;

    private ProxyConnection(
        TcpClient socketClient,
        ProxyCaptureWriter trafficCapture,
        ClientSessionLedger sessionLedger,
        string? sessionAccountIdentifier,
        ILogger? logger,
        CancellationToken cancellationToken
    )
    {
        SocketClient = socketClient;
        _homeVisitor = new ProxyHomeVisitor(this);
        SocketUpstream = new TcpClient();
        ClientStream = new MessageStream(socketClient.GetStream());
        EventBus = new EventBus();
        CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        TrafficCapture = trafficCapture;
        RemoteEndPoint = socketClient.GetRemoteEndPoint();
        _handshake = new ProxyHandshake(sessionLedger, sessionAccountIdentifier, logger, RemoteEndPoint);
    }

    /// <summary>
    /// Gets the Cancellation Token Source value.
    /// </summary>
    public CancellationTokenSource CancellationTokenSource { get; }

    /// <summary>
    /// Gets the Client Stream value.
    /// </summary>
    public MessageStream ClientStream { get; }

    /// <summary>
    /// Gets the <c language="csharp">CompletionTask</c> value.
    /// </summary>
    public Task CompletionTask { get; private set; } = Task.CompletedTask;

    /// <summary>
    /// Gets the Event Bus value.
    /// </summary>
    public EventBus EventBus { get; }

    /// <summary>
    /// Gets the <c language="csharp">RemoteEndPoint</c> value.
    /// </summary>
    public string RemoteEndPoint { get; }

    /// <summary>
    /// Gets the Server Stream value.
    /// </summary>
    public MessageStream ServerStream =>
        _serverStream ?? throw new InvalidOperationException(message: "The upstream stream is unavailable.");

    /// <summary>
    /// Gets the Tcp Client value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("TcpClient")]
    public TcpClient SocketClient { get; }

    /// <summary>
    /// Gets the Tcp Upstream value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("TcpUpstream")]
    public TcpClient SocketUpstream { get; }

    /// <summary>
    /// Gets the Traffic Capture value.
    /// </summary>
    public ProxyCaptureWriter TrafficCapture { get; }

    /// <summary>
    /// Provides the Connect Async value or operation.
    /// </summary>
    public static async Task<ProxyConnection> ConnectAsync(
        TcpClient socketClient,
        string upstreamHost,
        int upstreamPort,
        ProxyCaptureWriter trafficCapture,
        ClientSessionLedger sessionLedger,
        string? sessionAccountIdentifier = null,
        IServerPublicKeySource? serverKeys = null,
        ICommandDataResolver? commandDataResolver = null,
        ILogger? logger = null,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(socketClient);
        ArgumentNullException.ThrowIfNull(sessionLedger);
        ProxyConnection client = new(socketClient, trafficCapture, sessionLedger, sessionAccountIdentifier, logger, cancellationToken);

        try
        {
            await client
                .SocketUpstream.ConnectAsync(upstreamHost, upstreamPort, client.CancellationTokenSource.Token)
                .ConfigureAwait(continueOnCapturedContext: false);
            client.ClientStream.CommandDataResolver = commandDataResolver;
            client._serverStream = new MessageStream(client.SocketUpstream.GetStream())
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
    /// Executes the <c language="csharp">RunAsync</c> operation.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        if (_started)
            throw new InvalidOperationException(message: "The proxy client is already running.");

        _started = true;
        await EventBus
            .SubscribeAsync<MessageReceivedEvent>(OnMessageReceivedEventAsync, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
        await EventBus
            .SubscribeAsync<MessageSentEvent>(OnMessageSentEventAsync, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        try
        {
            // Handshake handlers must be subscribed before either pump can observe ServerHello or Login.
            CompletionTask = RunPumpsAsync(CancellationTokenSource.Token);
            await CompletionTask.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            await EventBus
                .UnsubscribeAsync<MessageReceivedEvent>(OnMessageReceivedEventAsync, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            await EventBus
                .UnsubscribeAsync<MessageSentEvent>(OnMessageSentEventAsync, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    /// <summary>
    /// Executes the <c language="csharp">ToString</c> operation.
    /// </summary>
    public override string ToString()
    {
        return RemoteEndPoint;
    }

    /// <summary>
    /// Executes the <c language="csharp">VisitHomeAsync</c> operation.
    /// </summary>
    public ValueTask<OtherHomeDataMessage> VisitHomeAsync(LongIdentifier target, CancellationToken cancellationToken = default)
    {
        return _homeVisitor.VisitHomeAsync(target, cancellationToken);
    }

    internal async Task WriteMessageAsync(IMessage message, MessageDirection direction, CancellationToken cancellationToken = default)
    {
        MessageStream source = direction is MessageDirection.Clientbound ? ServerStream : ClientStream;
        MessageStream destination = direction is MessageDirection.Clientbound ? ClientStream : ServerStream;

        MessageContainer outgoingContainer = message.ToContainer(MessageRegistry.GetIdentifier(message), MessageRegistry.GetVersion(message));

        await TrafficCapture
            .SaveAsync(stage: "outgoing", direction, outgoingContainer, MessageRegistry.GetCaptureName(message), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
        await destination
            .WriteContainerAsync(outgoingContainer, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
        await EventBus
            .PublishAsync(new MessageSentEvent(message, direction, source, destination), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>
    /// Executes the <c language="csharp">DisposeAsync</c> operation.
    /// </summary>
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        ClientStream.Dispose();
        _serverStream?.Dispose();

        SocketClient.Dispose();
        SocketUpstream.Dispose();
        CancellationTokenSource.Dispose();

        await CompletionTask.ConfigureAwait(continueOnCapturedContext: false);

    }

    private async Task OnMessageReceivedEventAsync(MessageReceivedEvent @event, CancellationToken cancellationToken = default)
    {
        await _handshake.OnMessageReceivedEventAsync(@event, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        switch (@event.Message)
        {
            case VisitHomeTargetMessage visitHomeTargetMessage
                when visitHomeTargetMessage.Target == LongIdentifier.Empty:
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
            Console.WriteLine(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"[{DateTime.Now:T}] CANCELLED {@event.Direction} {(@event.Message is PassthroughMessage ? @event.Message : string.Empty)}"
                )
            );
        }
    }

    private async Task OnMessageSentEventAsync(MessageSentEvent @event, CancellationToken cancellationToken = default)
    {
        await _handshake.OnMessageSentEventAsync(@event, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (@event.Message is EndClientTurnMessage endClientTurnMessage)
        {
            Console.WriteLine(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"[{DateTime.Now:T}] {nameof(EndClientTurnMessage)} {{ Checksum = {endClientTurnMessage.Checksum}, SubTick = {endClientTurnMessage.SubTick}, SubChecksums = [{string.Join(separator: ',', endClientTurnMessage.SubChecksums.ToArray())}], Commands = [{string.Join(separator: ',', endClientTurnMessage.Commands.ToArray().Select(static command => command.Type))}] }}"
                )
            );
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
        MessageStream source = direction is MessageDirection.Clientbound ? ServerStream : ClientStream;
        MessageStream destination = direction is MessageDirection.Clientbound ? ClientStream : ServerStream;

        while (!cancellationToken.IsCancellationRequested)
        {
            IMessage message = await ReadForwardMessageAsync(source, direction, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            MessageReceivedEvent @event = new(message, direction, source, destination);
            await EventBus
                .PublishAsync(@event, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (@event.IsCancelled)
                continue;

            await WriteForwardMessageAsync(message, destination, direction, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await EventBus
                .PublishAsync(new MessageSentEvent(message, direction, source, destination), cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private async Task<IMessage> ReadForwardMessageAsync(MessageStream source, MessageDirection direction, CancellationToken cancellationToken)
    {
        try
        {
            MessageContainer container = await source
                .ReadContainerAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await TrafficCapture
                .SaveAsync(stage: "incoming", direction, container, messageName: "frame", cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            return source.ResolveMessage(container);
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
            Console.WriteLine(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"[{DateTime.Now:T}] {remoteEndPoint} closed: messages were encrypted likely with an incorrect public key"
                )
            );
        }
        catch (StreamClosedException exception)
        {
            Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"[{DateTime.Now:T}] {remoteEndPoint} closed: {exception.Message}"));
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine($"{remoteEndPoint} proxy connection stopped.");
        }
        catch (Exception exception)
            when (ProxyFailureClassifier.IsRecoverable(exception))
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

    private async Task WriteForwardMessageAsync(IMessage message, MessageStream destination, MessageDirection direction, CancellationToken cancellationToken)
    {
        MessageContainer container = message.ToContainer(MessageRegistry.GetIdentifier(message), MessageRegistry.GetVersion(message));

        await TrafficCapture
            .SaveAsync(stage: "outgoing", direction, container, MessageRegistry.GetCaptureName(message), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        try
        {
            await destination
                .WriteContainerAsync(container, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (StreamClosedException exception)
        {
            string side = direction is MessageDirection.Clientbound ? "client" : "server";

            throw new StreamClosedException($"{side} closed the connection", exception);
        }
    }
}
