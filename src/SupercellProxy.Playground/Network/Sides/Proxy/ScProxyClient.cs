using SupercellProxy.Playground.Crypto.Exceptions;
using SupercellProxy.Playground.Events;
using SupercellProxy.Playground.Events.Bus;
using SupercellProxy.Playground.Exceptions;
using SupercellProxy.Playground.Extensions;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Supercell;
using System.Net.Sockets;

namespace SupercellProxy.Playground.Network.Sides.Proxy;

public record ScProxyClient(TcpClient TcpClient, TcpClient TcpUpstream, SupercellStream ClientStream, SupercellStream ServerStream, EventBus EventBus, CancellationTokenSource CancellationTokenSource) : IAsyncDisposable
{
    private bool _suppressEndClientTurns = false;

    public string RemoteEndPoint { get; } = TcpClient.RemoteEndPoint;
    public Task CompletionTask { get; init; } = Task.Run(async () =>
    {
        var serverboundPumpTask = PumpAsync(Direction.Serverbound, CancellationTokenSource.Token);
        var clientboundPumpTask = PumpAsync(Direction.Clientbound, CancellationTokenSource.Token);

        var remoteEndPoint = TcpClient.RemoteEndPoint;
        var completedTask = await Task.WhenAny(serverboundPumpTask, clientboundPumpTask);

        try
        {
            await completedTask;
        }
        catch (MacVerificationException exception) when (exception.IsPublicKeyBox)
        {
            Console.WriteLine($"[{DateTime.Now:T}] {remoteEndPoint} closed: messages were encrypted likely with an incorrect public key");
        }
        catch (StreamClosedException exception)
        {
            Console.WriteLine($"[{DateTime.Now:T}] {remoteEndPoint} closed: {exception.Message}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"[{DateTime.Now:T}] {remoteEndPoint} closed: {exception}");
        }
        finally
        {
            await CancellationTokenSource.CancelAsync();

            try
            {
                if (completedTask == serverboundPumpTask)
                    await clientboundPumpTask;
                else
                    await serverboundPumpTask;
            }
            catch (TaskCanceledException)
            {
                // Ignored
            }
        }

        return;

        async Task PumpAsync(Direction direction, CancellationToken cancellationToken = default)
        {
            var source = direction is Direction.Clientbound ? ServerStream : ClientStream;
            var destination = direction is Direction.Clientbound ? ClientStream : ServerStream;

            while (!cancellationToken.IsCancellationRequested)
            {
                IMessage message;

                try
                {
                    message = await source.ReadMessageAsync(cancellationToken);
                }
                catch (StreamClosedException exception)
                {
                    var side = direction is Direction.Clientbound ? "server" : "client";
                    throw new StreamClosedException($"{side} closed the connection", exception);
                }

                var @event = await EventBus.PublishAsync(new MessageReceivedEvent(message, direction, source, destination), cancellationToken);

                if (@event.IsCancelled)
                    continue;

                try
                {
                    await destination.WriteMessageAsync(message, cancellationToken);
                }
                catch (StreamClosedException exception)
                {
                    var side = direction is Direction.Clientbound ? "client" : "server";
                    throw new StreamClosedException($"{side} closed the connection", exception);
                }

                await EventBus.PublishAsync(new MessageSentEvent(message, direction, source, destination), cancellationToken);
            }
        }
    });

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await EventBus.SubscribeAsync<MessageReceivedEvent>(OnMessageReceivedEvent, cancellationToken);
        await EventBus.SubscribeAsync<MessageSentEvent>(OnMessageSentEvent, cancellationToken);

        await Task.Delay(15_000, cancellationToken);

        try
        {
            var accountId = LogicLong.Parse("#Q2V0U29JQ");

            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"[{DateTime.Now:T}] Visiting {accountId} home...");
                var otherHomeDataMessage = await VisitHomeAsync(accountId, cancellationToken);

                if (!File.Exists("other_home_data_message.bin"))
                    await File.WriteAllBytesAsync("other_home_data_message.bin", otherHomeDataMessage.UnknownData, cancellationToken);

                await Task.Delay(1_000, cancellationToken);
                accountId--;
            }
        }
        finally
        {
            await EventBus.UnsubscribeAsync<MessageReceivedEvent>(OnMessageReceivedEvent, cancellationToken);
            await EventBus.UnsubscribeAsync<MessageSentEvent>(OnMessageSentEvent, cancellationToken);
        }
    }

    public async ValueTask<OtherHomeDataMessage> VisitHomeAsync(LogicLong target, CancellationToken cancellationToken = default)
    {
        _suppressEndClientTurns = true;

        try
        {
            await WriteMessageAsync(new VisitHomeMessage
            {
                Unknown0 = 0x01,
                Unknown1 = 0x02
            }, Direction.Serverbound, cancellationToken);

            await WriteMessageAsync(new VisitHomeTargetMessage
            {
                Unknown0 = 0x00,
                Target = target
            }, Direction.Serverbound, cancellationToken);

            var otherHomeDataMessage = await ExpectMessageAsync<OtherHomeDataMessage>(timeout: TimeSpan.FromSeconds(15), cancellationToken);

            do
            {
                try
                {
                    var endClientTurnMessage = await ExpectMessageAsync<EndClientTurnMessage>(timeout: TimeSpan.FromSeconds(3), cancellationToken);

                    if (endClientTurnMessage.SubTick is 0)
                        break;
                }
                catch (TimeoutException)
                {
                    break;
                }

            }
            while (!cancellationToken.IsCancellationRequested);

            return otherHomeDataMessage;
        }
        finally
        {
            _suppressEndClientTurns = false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await ClientStream.DisposeAsync();
        await ServerStream.DisposeAsync();

        TcpClient.Dispose();
        TcpUpstream.Dispose();

        await CompletionTask;

        GC.SuppressFinalize(this);
    }

    public override string ToString()
    {
        return RemoteEndPoint;
    }

    private async Task WriteMessageAsync(IMessage message, Direction direction, CancellationToken cancellationToken = default)
    {
        var source = direction is Direction.Clientbound ? ServerStream : ClientStream;
        var destination = direction is Direction.Clientbound ? ClientStream : ServerStream;

        await destination.WriteMessageAsync(message, cancellationToken);
        await EventBus.PublishAsync(new MessageSentEvent(message, direction, source, destination), cancellationToken);
    }

    private async Task OnMessageReceivedEvent(MessageReceivedEvent @event, CancellationToken cancellationToken = default)
    {
        var fileName = $"packet_{MessageRegistry.GetId(@event.Message)}.bin";

        if (!File.Exists(fileName))
            await File.WriteAllBytesAsync(fileName, @event.Message.ToContainer(MessageRegistry.GetId(@event.Message), MessageRegistry.GetVersion(@event.Message)).Payload.ToArray(), cancellationToken);


        switch (@event.Message)
        {
            case ServerHelloMessage serverHelloMessage:
                await @event.Source.SetupEncryptionAsync(Side.Server, serverHelloMessage.SessionKey, cancellationToken);
                break;
            case VisitHomeTargetMessage visitHomeTargetMessage when visitHomeTargetMessage.Target == LogicLong.Empty:
                @event.IsCancelled = true;
                break;
            case EndClientTurnMessage when _suppressEndClientTurns:
                @event.IsCancelled = true;
                break;
        }

        if (@event.IsCancelled)
            Console.WriteLine($"[{DateTime.Now:T}] CANCELLED {@event.Direction} {(@event.Message is PassthroughMessage ? @event.Message : string.Empty)}");
    }

    private async Task OnMessageSentEvent(MessageSentEvent @event, CancellationToken cancellationToken = default)
    {
        switch (@event.Message)
        {
            case ServerHelloMessage serverHelloMessage:
                await @event.Destination.SetupEncryptionAsync(Side.Client, serverHelloMessage.SessionKey, cancellationToken);
                break;
        }

        if (@event.Message is PassthroughMessage)
            Console.WriteLine($"[{DateTime.Now:T}] {@event.Direction} {@event.Message}");
        else
            Console.WriteLine($"[{DateTime.Now:T}] {@event.Message}");
    }

    private async Task<TMessage> ExpectMessageAsync<TMessage>(TimeSpan timeout, CancellationToken cancellationToken = default) where TMessage : class, IMessage
    {
        using var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        linkedCancellationTokenSource.CancelAfter(timeout);

        try
        {
            return await ExpectMessageAsync<TMessage>(linkedCancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
            throw new TimeoutException($"Expected message of type {typeof(TMessage)} was not received within the timeout period of {timeout}");
        }
    }

    private async Task<TMessage> ExpectMessageAsync<TMessage>(CancellationToken cancellationToken = default) where TMessage : class, IMessage
    {
        var taskCompletionSource = new TaskCompletionSource<TMessage>();
        Func<MessageSentEvent, CancellationToken, Task> handler = OnMessageSentEvent;
        await EventBus.SubscribeAsync<MessageSentEvent>(handler, cancellationToken);

        try
        {
            return await taskCompletionSource.Task.WaitAsync(cancellationToken);
        }
        finally
        {
            await EventBus.UnsubscribeAsync<MessageSentEvent>(handler, cancellationToken);
        }

        Task OnMessageSentEvent(MessageSentEvent @event, CancellationToken cancellationToken = default)
        {
            if (@event.Message is not TMessage message)
                return Task.CompletedTask;

            taskCompletionSource.TrySetResult(message);

            return Task.CompletedTask;
        }
    }
}
