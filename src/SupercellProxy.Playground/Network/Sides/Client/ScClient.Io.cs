using SupercellProxy.Playground.Network.Streams;
using System.Net.Sockets;

namespace SupercellProxy.Playground.Network.Sides;

public partial class ScClient
{
    private TcpClient? tcpClient;
    private NetworkStream? _networkStream;
    private SupercellStream? _supercellStream;

    private async Task HandleIncomingMessageAsync(CancellationToken cancellationToken = default)
    {
        var stream = await GetStreamAsync(cancellationToken);
        var message = await stream.ReadMessageAsync(cancellationToken);
        Console.WriteLine($"Received message: {message}");
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
