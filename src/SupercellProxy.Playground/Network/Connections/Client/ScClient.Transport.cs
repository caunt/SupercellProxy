using System.Globalization;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Home;
using SupercellProxy.Playground.Home.Simulation;
using SupercellProxy.Playground.Json;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Configuration;
using SupercellProxy.Playground.Network.Connections.Client.Exceptions;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Protocol;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Connections.Client;

public partial class ScClient
{
    private async Task HandleIncomingMessageAsync(CancellationToken cancellationToken = default)
    {
        var stream = await GetStreamAsync(cancellationToken).ConfigureAwait(false);
        var message = await stream.ReadMessageAsync(cancellationToken).ConfigureAwait(false);

        Console.WriteLine($"Received message: {message}");
    }

    private async Task<MessageStream> GetStreamAsync(CancellationToken cancellationToken = default)
    {
        if (_supercellStream is null)
        {
            tcpClient = new TcpClient();
            await tcpClient
                .ConnectAsync(
                    configuration.UpstreamHost,
                    configuration.UpstreamPort,
                    cancellationToken
                )
                .ConfigureAwait(false);

            _networkStream = tcpClient.GetStream();
            _supercellStream = new MessageStream(_networkStream);
        }

        return _supercellStream;
    }

    private async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (_supercellStream is not null)
        {
            await _supercellStream.DisposeAsync().ConfigureAwait(false);
            _supercellStream = null;
        }

        if (_networkStream is not null)
        {
            await _networkStream.DisposeAsync().ConfigureAwait(false);
            _networkStream = null;
        }

        tcpClient?.Dispose();
        tcpClient = null;
    }
}
