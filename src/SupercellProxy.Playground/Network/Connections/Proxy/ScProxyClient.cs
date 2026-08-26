using System.Globalization;
using System.Net.Sockets;
using SupercellProxy.Playground.Crypto.Exceptions;
using SupercellProxy.Playground.Events;
using SupercellProxy.Playground.Events.Bus;
using SupercellProxy.Playground.Extensions;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Connections.Client;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Protocol;
using SupercellProxy.Playground.Network.Transport;
using SupercellProxy.Playground.Network.Transport.Exceptions;

namespace SupercellProxy.Playground.Network.Connections.Proxy;

/// <summary>
/// Represents <c>ScProxyClient</c>.
/// </summary>
public record ScProxyClient(
    TcpClient TcpClient,
    TcpClient TcpUpstream,
    MessageStream ClientStream,
    MessageStream ServerStream,
    EventBus EventBus,
    CancellationTokenSource CancellationTokenSource,
    ProxyTrafficCapture TrafficCapture
) : IAsyncDisposable
{
    private LoginMessage? _loginMessage;
    private bool _suppressEndClientTurns = false;
    private bool _started;

    /// <summary>
    /// Gets the <c>RemoteEndPoint</c> value.
    /// </summary>
    public string RemoteEndPoint { get; } = TcpClient.GetRemoteEndPoint();

    /// <summary>
    /// Gets the <c>CompletionTask</c> value.
    /// </summary>
    public Task CompletionTask { get; private set; } = Task.CompletedTask;

    /// <summary>
    /// Executes the <c>RunAsync</c> operation.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        if (_started)
            throw new InvalidOperationException("The proxy client is already running.");

        _started = true;
        await EventBus
            .SubscribeAsync<MessageReceivedEvent>(OnMessageReceivedEventAsync, cancellationToken)
            .ConfigureAwait(false);
        await EventBus
            .SubscribeAsync<MessageSentEvent>(OnMessageSentEventAsync, cancellationToken)
            .ConfigureAwait(false);

        try
        {
            // Handshake handlers must be subscribed before either pump can observe ServerHello or Login.
            CompletionTask = RunPumpsAsync(CancellationTokenSource.Token);
            await CompletionTask.WaitAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            await EventBus
                .UnsubscribeAsync<MessageReceivedEvent>(
                    OnMessageReceivedEventAsync,
                    cancellationToken
                )
                .ConfigureAwait(false);
            await EventBus
                .UnsubscribeAsync<MessageSentEvent>(OnMessageSentEventAsync, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private async Task RunPumpsAsync(CancellationToken cancellationToken)
    {
        var serverboundPumpTask = PumpAsync(Direction.Serverbound, cancellationToken);
        var clientboundPumpTask = PumpAsync(Direction.Clientbound, cancellationToken);

        var remoteEndPoint = TcpClient.GetRemoteEndPoint();
        var completedTask = await Task.WhenAny(serverboundPumpTask, clientboundPumpTask)
            .ConfigureAwait(false);

        try
        {
            await completedTask.ConfigureAwait(false);
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
            Console.WriteLine(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"[{DateTime.Now:T}] {remoteEndPoint} closed: {exception.Message}"
                )
            );
        }
        catch (Exception exception)
        {
            Console.WriteLine(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"[{DateTime.Now:T}] {remoteEndPoint} closed: {exception}"
                )
            );
        }
        finally
        {
            await CancellationTokenSource.CancelAsync().ConfigureAwait(false);

            try
            {
                if (completedTask == serverboundPumpTask)
                    await clientboundPumpTask.ConfigureAwait(false);
                else
                    await serverboundPumpTask.ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                // Ignored
            }
        }
    }

    private async Task PumpAsync(Direction direction, CancellationToken cancellationToken)
    {
        var source = direction is Direction.Clientbound ? ServerStream : ClientStream;
        var destination = direction is Direction.Clientbound ? ClientStream : ServerStream;

        while (!cancellationToken.IsCancellationRequested)
        {
            var message = await ReadForwardMessageAsync(source, direction, cancellationToken)
                .ConfigureAwait(false);

            var @event = await EventBus
                .PublishAsync(
                    new MessageReceivedEvent(message, direction, source, destination),
                    cancellationToken
                )
                .ConfigureAwait(false);

            if (@event.IsCancelled)
                continue;

            await WriteForwardMessageAsync(message, destination, direction, cancellationToken)
                .ConfigureAwait(false);

            await EventBus
                .PublishAsync(
                    new MessageSentEvent(message, direction, source, destination),
                    cancellationToken
                )
                .ConfigureAwait(false);
        }
    }

    private async Task<IMessage> ReadForwardMessageAsync(
        MessageStream source,
        Direction direction,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var container = await source
                .ReadContainerAsync(cancellationToken)
                .ConfigureAwait(false);
            await TrafficCapture
                .SaveAsync("incoming", direction, container, "frame", cancellationToken)
                .ConfigureAwait(false);
            return MessageRegistry.Resolve(container, source.CommandDataResolver);
        }
        catch (StreamClosedException exception)
        {
            var side = direction is Direction.Clientbound ? "server" : "client";
            throw new StreamClosedException($"{side} closed the connection", exception);
        }
    }

    private async Task WriteForwardMessageAsync(
        IMessage message,
        MessageStream destination,
        Direction direction,
        CancellationToken cancellationToken
    )
    {
        var container = message.ToContainer(
            MessageRegistry.GetId(message),
            MessageRegistry.GetVersion(message)
        );
        await TrafficCapture
            .SaveAsync("outgoing", direction, container, message.GetType().Name, cancellationToken)
            .ConfigureAwait(false);

        try
        {
            await destination
                .WriteContainerAsync(container, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (StreamClosedException exception)
        {
            var side = direction is Direction.Clientbound ? "client" : "server";
            throw new StreamClosedException($"{side} closed the connection", exception);
        }
    }

    /// <summary>
    /// Executes the <c>VisitHomeAsync</c> operation.
    /// </summary>
    public async ValueTask<OtherHomeDataMessage> VisitHomeAsync(
        LongId target,
        CancellationToken cancellationToken = default
    )
    {
        _suppressEndClientTurns = true;

        try
        {
            await WriteMessageAsync(
                    new VisitHomeMessage { Unknown0 = 0x01, Unknown1 = 0x02 },
                    Direction.Serverbound,
                    cancellationToken
                )
                .ConfigureAwait(false);

            await WriteMessageAsync(
                    new VisitHomeTargetMessage { Unknown0 = 0x00, Target = target },
                    Direction.Serverbound,
                    cancellationToken
                )
                .ConfigureAwait(false);

            var otherHomeDataMessage = await ExpectMessageAsync<OtherHomeDataMessage>(
                    timeout: TimeSpan.FromSeconds(15),
                    cancellationToken
                )
                .ConfigureAwait(false);

            do
            {
                try
                {
                    var endClientTurnMessage = await ExpectMessageAsync<EndClientTurnMessage>(
                            timeout: TimeSpan.FromSeconds(3),
                            cancellationToken
                        )
                        .ConfigureAwait(false);

                    if (endClientTurnMessage.SubTick is 0)
                        break;
                }
                catch (TimeoutException)
                {
                    break;
                }
            } while (!cancellationToken.IsCancellationRequested);

            return otherHomeDataMessage;
        }
        finally
        {
            _suppressEndClientTurns = false;
        }
    }

    /// <summary>
    /// Executes the <c>DisposeAsync</c> operation.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await ClientStream.DisposeAsync().ConfigureAwait(false);
        await ServerStream.DisposeAsync().ConfigureAwait(false);

        TcpClient.Dispose();
        TcpUpstream.Dispose();

        await CompletionTask.ConfigureAwait(false);

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Executes the <c>ToString</c> operation.
    /// </summary>
    public override string ToString()
    {
        return RemoteEndPoint;
    }

    private async Task WriteMessageAsync(
        IMessage message,
        Direction direction,
        CancellationToken cancellationToken = default
    )
    {
        var source = direction is Direction.Clientbound ? ServerStream : ClientStream;
        var destination = direction is Direction.Clientbound ? ClientStream : ServerStream;

        var outgoingContainer = message.ToContainer(
            MessageRegistry.GetId(message),
            MessageRegistry.GetVersion(message)
        );
        await TrafficCapture
            .SaveAsync(
                "outgoing",
                direction,
                outgoingContainer,
                message.GetType().Name,
                cancellationToken
            )
            .ConfigureAwait(false);
        await destination
            .WriteContainerAsync(outgoingContainer, cancellationToken)
            .ConfigureAwait(false);
        await EventBus
            .PublishAsync(
                new MessageSentEvent(message, direction, source, destination),
                cancellationToken
            )
            .ConfigureAwait(false);
    }

    private async Task OnMessageReceivedEventAsync(
        MessageReceivedEvent @event,
        CancellationToken cancellationToken = default
    )
    {
        switch (@event.Message)
        {
            case LoginMessage loginMessage when @event.Direction is Direction.Serverbound:
                _loginMessage = loginMessage;
                break;
            case ServerHelloMessage serverHelloMessage:
                await @event
                    .Source.SetupEncryptionAsync(
                        Side.Server,
                        serverHelloMessage.SessionKey,
                        cancellationToken
                    )
                    .ConfigureAwait(false);
                break;
            case VisitHomeTargetMessage visitHomeTargetMessage
                when visitHomeTargetMessage.Target == LongId.Empty:
                @event.IsCancelled = true;
                break;
            case EndClientTurnMessage when _suppressEndClientTurns:
                @event.IsCancelled = true;
                break;
        }

        if (@event.IsCancelled)
            Console.WriteLine(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"[{DateTime.Now:T}] CANCELLED {@event.Direction} {(@event.Message is PassthroughMessage ? @event.Message : string.Empty)}"
                )
            );
    }

    private async Task OnMessageSentEventAsync(
        MessageSentEvent @event,
        CancellationToken cancellationToken = default
    )
    {
        switch (@event.Message)
        {
            case LoginOkMessage loginOkMessage
                when @event.Direction is Direction.Clientbound && _loginMessage is { } loginMessage:
                await ScClientSession
                    .SaveAsync(
                        loginOkMessage.AccountId,
                        loginOkMessage.PassToken,
                        loginMessage.AppStore,
                        loginMessage.CompressedData,
                        cancellationToken: cancellationToken
                    )
                    .ConfigureAwait(false);
                break;
            case ServerHelloMessage serverHelloMessage:
                await @event
                    .Destination.SetupEncryptionAsync(
                        Side.Client,
                        serverHelloMessage.SessionKey,
                        cancellationToken
                    )
                    .ConfigureAwait(false);
                break;
        }

        if (@event.Message is EndClientTurnMessage endClientTurnMessage)
        {
            Console.WriteLine(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"[{DateTime.Now:T}] {nameof(EndClientTurnMessage)} {{ Checksum = {endClientTurnMessage.Checksum}, SubTick = {endClientTurnMessage.SubTick}, SubChecksums = [{string.Join(',', endClientTurnMessage.SubChecksums.ToArray())}], Commands = [{string.Join(',', endClientTurnMessage.Commands.ToArray().Select(static command => command.Type))}] }}"
                )
            );
        }
        else if (@event.Message is PassthroughMessage)
            Console.WriteLine(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"[{DateTime.Now:T}] {@event.Direction} {@event.Message}"
                )
            );
        else
            Console.WriteLine(
                string.Create(CultureInfo.InvariantCulture, $"[{DateTime.Now:T}] {@event.Message}")
            );
    }

    private async Task<TMessage> ExpectMessageAsync<TMessage>(
        TimeSpan timeout,
        CancellationToken cancellationToken = default
    )
        where TMessage : class, IMessage
    {
        using var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken
        );
        linkedCancellationTokenSource.CancelAfter(timeout);

        try
        {
            return await ExpectMessageAsync<TMessage>(linkedCancellationTokenSource.Token)
                .ConfigureAwait(false);
        }
        catch (TaskCanceledException)
        {
            throw new TimeoutException(
                $"Expected message of type {typeof(TMessage)} was not received within the timeout period of {timeout}"
            );
        }
    }

    private async Task<TMessage> ExpectMessageAsync<TMessage>(
        CancellationToken cancellationToken = default
    )
        where TMessage : class, IMessage
    {
        var taskCompletionSource = new TaskCompletionSource<TMessage>();
        Func<MessageSentEvent, CancellationToken, Task> handler = OnMessageSentEventAsync;
        await EventBus.SubscribeAsync(handler, cancellationToken).ConfigureAwait(false);

        try
        {
            return await taskCompletionSource
                .Task.WaitAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            await EventBus.UnsubscribeAsync(handler, cancellationToken).ConfigureAwait(false);
        }

        Task OnMessageSentEventAsync(
            MessageSentEvent @event,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                if (@event.Message is not TMessage message)
                    return Task.CompletedTask;

                taskCompletionSource.TrySetResult(message);
                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                return Task.FromException(exception);
            }
        }
    }
}
