using System.Globalization;
using System.Net;
using System.Net.Sockets;
using SupercellProxy.Playground.Crypto.Exceptions;
using SupercellProxy.Playground.Extensions;
using SupercellProxy.Playground.Network.Configuration;

namespace SupercellProxy.Playground.Network.Connections.Proxy;

/// <summary>
/// Represents <c language="csharp">ScProxy</c>.
/// </summary>
/// <remarks>
/// Initializes a new <see cref="ScProxy"/> instance.
/// </remarks>
internal sealed class ScProxy(ProxyConfiguration configuration)
{
    private readonly ProxyConfiguration _configuration = configuration;

    /// Runs the proxy using command-line connection arguments.
    public static async Task RunAsync(
        string[] arguments,
        CancellationToken cancellationToken = default
    )
    {
        var (upstreamHost, upstreamPort) = await ConnectionAddress
            .ResolveAsync(arguments, cancellationToken)
            .ConfigureAwait(false);
        var proxy = new ScProxy(
            new ProxyConfiguration(
                upstreamHost,
                upstreamPort,
                arguments.ElementAtOrDefault(2) ?? ConnectionAddress.DefaultListenHost,
                ConnectionAddress.ParsePort(arguments.ElementAtOrDefault(3)),
                ProtocolConfiguration.Current
            )
        );
        await proxy.RunAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Defines the <c language="csharp">StandardPrivateKey</c> value.
    /// </summary>
    public static readonly byte[] StandardPrivateKey =
    [
        0x18,
        0x91,
        0xD4,
        0x01,
        0xFA,
        0xDB,
        0x51,
        0xD2,
        0x5D,
        0x3A,
        0x91,
        0x74,
        0xD4,
        0x72,
        0xA9,
        0xF6,
        0x91,
        0xA4,
        0x5B,
        0x97,
        0x42,
        0x85,
        0xD4,
        0x77,
        0x29,
        0xC4,
        0x5C,
        0x65,
        0x38,
        0x07,
        0x0D,
        0x85,
    ];

    /// <summary>
    /// Executes the <c language="csharp">RunAsync</c> operation.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        using var listener = new TcpListener(
            IPAddress.Parse(_configuration.ListenAddress),
            _configuration.ListenPort
        );
        listener.Start();

        Console.WriteLine(
            string.Create(
                CultureInfo.InvariantCulture,
                $"[{DateTime.Now:T}] Listening on {_configuration.ListenAddress}:{_configuration.ListenPort}, upstream {_configuration.UpstreamHost}:{_configuration.UpstreamPort}"
            )
        );

        while (!cancellationToken.IsCancellationRequested)
        {
            var client = await listener
                .AcceptTcpClientAsync(cancellationToken)
                .ConfigureAwait(false);
            _ = Task.Run(
                async () =>
                {
                    try
                    {
                        await RunClientAsync(client, cancellationToken).ConfigureAwait(false);
                    }
                    catch (Exception exception)
                        when (exception
                                is IOException
                                    or SocketException
                                    or InvalidDataException
                                    or OperationCanceledException
                                    or TimeoutException
                                    or NaClV3Exception
                        )
                    {
                        Console.WriteLine(exception);
                    }
                },
                CancellationToken.None
            );
        }
    }

    private async Task RunClientAsync(
        TcpClient tcpClient,
        CancellationToken cancellationToken = default
    )
    {
        Console.WriteLine(
            string.Create(
                CultureInfo.InvariantCulture,
                $"[{DateTime.Now:T}] Incoming connection from {tcpClient.GetRemoteEndPoint()}"
            )
        );
        var trafficCapture = new ProxyTrafficCapture(
            ProxyTrafficCapture.RootDirectoryPath,
            tcpClient.GetRemoteEndPoint()
        );
        Console.WriteLine(
            string.Create(
                CultureInfo.InvariantCulture,
                $"[{DateTime.Now:T}] Saving proxy traffic to {trafficCapture.DirectoryPath}"
            )
        );
        var client = await ScProxyClient
            .ConnectAsync(
                tcpClient,
                _configuration.UpstreamHost,
                _configuration.UpstreamPort,
                trafficCapture,
                cancellationToken
            )
            .ConfigureAwait(false);
        await using (client.ConfigureAwait(false))
        {
            try
            {
                await client.RunAsync(client.CancellationTokenSource.Token).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                // Ignored
            }
            finally
            {
                await client.CompletionTask.ConfigureAwait(false);
            }
        }
    }
}
