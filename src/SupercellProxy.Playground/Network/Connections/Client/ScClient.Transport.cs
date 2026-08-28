using System.Net.Sockets;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Connections.Client;

internal sealed partial class ScClient
{
    private async Task HandleIncomingMessageAsync(CancellationToken cancellationToken = default)
    {
        var stream = await GetStreamAsync(cancellationToken).ConfigureAwait(false);
        var message = await stream.ReadMessageAsync(cancellationToken).ConfigureAwait(false);

        await ProcessClientboundAsync(message, cancellationToken).ConfigureAwait(false);
        Console.WriteLine($"Received message: {message}");
    }

    private async Task<MessageStream> GetStreamAsync(CancellationToken cancellationToken = default)
    {
        if (_supercellStream is null)
        {
            _tcpClient = new TcpClient();
            await _tcpClient
                .ConnectAsync(
                    Configuration.UpstreamHost,
                    Configuration.UpstreamPort,
                    cancellationToken
                )
                .ConfigureAwait(false);

            _networkStream = _tcpClient.GetStream();
            _supercellStream = new MessageStream(_networkStream);
        }

        return _supercellStream;
    }

    private async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (_supercellStream is not null)
        {
            _supercellStream.Dispose();
            _supercellStream = null;
        }

        if (_networkStream is not null)
        {
            await _networkStream.DisposeAsync().ConfigureAwait(false);
            _networkStream = null;
        }

        _tcpClient?.Dispose();
        _tcpClient = null;
    }
}
