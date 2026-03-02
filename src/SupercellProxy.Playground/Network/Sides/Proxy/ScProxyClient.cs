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

        try
        {
            var accountId = AccountId.Parse("#Q2V0U29JQ");

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(15_000, cancellationToken);

                Console.WriteLine($"[{DateTime.Now:T}] Visiting {accountId} home...");
                var otherHomeDataMessage = await VisitHomeAsync(accountId, cancellationToken);

                Console.WriteLine($"[{DateTime.Now:T}] Received {otherHomeDataMessage} for {accountId}");
            }
        }
        finally
        {
            await EventBus.UnsubscribeAsync<MessageReceivedEvent>(OnMessageReceivedEvent, cancellationToken);
            await EventBus.UnsubscribeAsync<MessageSentEvent>(OnMessageSentEvent, cancellationToken);
        }
    }

    public async ValueTask<OtherHomeDataMessage> VisitHomeAsync(AccountId target, CancellationToken cancellationToken = default)
    {
        await WriteMessageAsync(new VisitHomeMessage
        {
            Unknown0 = 0x01,
            Unknown1 = 0x02
        }, Direction.Serverbound, cancellationToken);

        // await WriteMessageAsync(new EndClientTurnMessage
        // {
        // 
        // }, Direction.Serverbound, cancellationToken);

        await WriteMessageAsync(new VisitHomeTargetMessage
        {
            Unknown0 = 0x00,
            Target = target
        }, Direction.Serverbound, cancellationToken);

        // await WriteMessageAsync(new PassthroughMessage
        // {
        //     Id = 38000,
        //     Version = 5213,
        //     Data = new byte[231]
        // }, Direction.Serverbound, cancellationToken);
        // 
        // using var memoryStream = new MemoryStream();
        // using var payloadStream = new SupercellStream(memoryStream);
        // 
        // payloadStream.WriteInt32(2);
        // using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal, leaveOpen: true))
        //     gzipStream.Write("{}"u8);
        // 
        // await WriteMessageAsync(new PassthroughMessage
        // {
        //     Id = 17339,
        //     Version = 5213,
        //     Data = memoryStream.ToArray()
        // }, Direction.Serverbound, cancellationToken);

        return await ExpectMessageAsync<OtherHomeDataMessage>(TimeSpan.FromSeconds(15), cancellationToken);
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
        switch (@event.Message)
        {
            case ServerHelloMessage serverHelloMessage:
                await @event.Source.SetupEncryptionAsync(Side.Server, serverHelloMessage.SessionKey, cancellationToken);
                break;
            case VisitHomeTargetMessage visitHomeTargetMessage when visitHomeTargetMessage.Target == AccountId.Empty:
                @event.IsCancelled = true;
                break;
            case PassthroughMessage passthroughMessage:
                var fileName = $"packet_{passthroughMessage.Id}.bin";

                if (!File.Exists(fileName))
                    await File.WriteAllBytesAsync(fileName, passthroughMessage.Data, cancellationToken);

                break;
        }

        if (@event.IsCancelled)
        {
            if (@event.Message is PassthroughMessage)
                Console.WriteLine($"[{DateTime.Now:T}] CANCELLED {@event.Direction} {@event.Message}");
            else
                Console.WriteLine($"[{DateTime.Now:T}] CANCELLED {@event.Message}");
        }
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
        return await ExpectMessageAsync<TMessage>(linkedCancellationTokenSource.Token);
    }

    private async Task<TMessage> ExpectMessageAsync<TMessage>(CancellationToken cancellationToken = default) where TMessage : class, IMessage
    {
        var taskCompletionSource = new TaskCompletionSource<TMessage>();
        await EventBus.SubscribeAsync<MessageSentEvent>(OnMessageSentEvent, cancellationToken);

        try
        {
            return await taskCompletionSource.Task.WaitAsync(cancellationToken);
        }
        finally
        {
            await EventBus.UnsubscribeAsync<MessageSentEvent>(OnMessageSentEvent, cancellationToken);
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
