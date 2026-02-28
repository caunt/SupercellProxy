using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Streams;
using System.Net.Sockets;

namespace SupercellProxy.Playground.Network.Sides;

public partial class Client
{
    private TcpClient? tcpClient;
    private NetworkStream? _networkStream;
    private SupercellStream? _supercellStream;

    private async Task HandleIncomingMessageAsync(CancellationToken cancellationToken = default)
    {
        var message = await ReadMessageAsync(cancellationToken);
        Console.WriteLine($"Received message: {message}");
    }

    private async Task WriteMessageAsync<T>(T message, CancellationToken cancellationToken = default) where T : IMessage
    {
        var stream = await GetStreamAsync(cancellationToken);
        await stream.WriteMessageAsync(message.ToContainer(MessageRegistry.GetId(message), version: MessageRegistry.GetVersion(message)), cancellationToken);
    }

    private async Task<IMessage> ReadMessageAsync(CancellationToken cancellationToken)
    {
        var stream = await GetStreamAsync(cancellationToken);
        var container = await stream.ReadMessageAsync(cancellationToken);
        var message = MessageRegistry.Resolve(container);

        if (container.Payload.Position != container.Payload.Length)
            Console.WriteLine($"Warning: Not all payload data was consumed for message {message}. Remaining bytes: {container.Payload.Length - container.Payload.Position}");

        return message;
    }

    private async Task<T> ReadMessageAsync<T>(CancellationToken cancellationToken) where T : IMessage
    {
        var genericMessage = await ReadMessageAsync(cancellationToken);

        if (genericMessage is not T message)
            throw new InvalidOperationException($"Expected message {typeof(T)}, but received {genericMessage}.");

        return message;
    }

    private async Task<SupercellStream> GetStreamAsync(CancellationToken cancellationToken = default)
    {
        if (_supercellStream is null)
        {
            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(configuration.UpstreamHost, configuration.UpstreamPort, cancellationToken);

            _networkStream = tcpClient.GetStream();
            _supercellStream = new SupercellStream(_networkStream);
        }

        return _supercellStream;
    }

    private async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (_supercellStream is not null)
        {
            await _supercellStream.DisposeAsync();
            _supercellStream = null;
        }

        if (_networkStream is not null)
        {
            await _networkStream.DisposeAsync();
            _networkStream = null;
        }

        tcpClient?.Dispose();
        tcpClient = null;
    }
}
