using System.Globalization;
using System.Net;
using System.Net.Sockets;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SupercellProxy.Networking.Assets;
using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Cryptography;
using SupercellProxy.Networking.Hosting;
using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Proxy;

/// <summary>
/// Represents <c language="csharp">ProtocolProxy</c>.
/// </summary>
/// <remarks>
/// Initializes a new <see cref="ProtocolProxy"/> instance.
/// </remarks>
public sealed class ProtocolProxy(
    IOptions<ProxyOptions> options,
    IServerPublicKeySource serverKeys,
    ILogger<ProtocolProxy> logger,
    TimeProvider timeProvider,
    ICommandDataResolver? commandDataResolver = null
)
{
    private readonly TaskCompletionSource<IPEndPoint> _listening = new(TaskCreationOptions.RunContinuationsAsynchronously);

    private ICommandDataResolver? _commandDataResolver = commandDataResolver;

    /// <summary>Completes with the actual endpoint when the listener starts, including an assigned ephemeral port.</summary>
    public Task<IPEndPoint> Listening => _listening.Task;

    /// <summary>
    /// Executes the <c language="csharp">RunAsync</c> operation.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Reliability",
        "CA2025:Ensure tasks complete before disposal",
        Justification = "All accepted connection tasks are retained and awaited in finally before the linked lifetime is disposed."
    )]
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        ProxyConfiguration configuration = options.Value.ToConfiguration();
        ClientSessionLedger sessionLedger = new(configuration.SessionLedgerPath);

        if (configuration.AssetDirectory is { } directory)
        {
            _commandDataResolver = await GameAssetDirectory
                .LoadAsync(directory, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        using TcpListener listener = new(IPAddress.Parse(configuration.ListenAddress), configuration.ListenPort);

        listener.Start();

        if (!_listening.TrySetResult((IPEndPoint)listener.LocalEndpoint))
            ConnectionLog.Debug(logger, message: "The proxy listening endpoint was already reported.");

        ConnectionLog.Write(
            logger,
            string.Create(
                CultureInfo.InvariantCulture,
                $"[{timeProvider.GetLocalNow():T}] Listening on {configuration.ListenAddress}:{configuration.ListenPort}, upstream {configuration.UpstreamHost}:{configuration.UpstreamPort}"
            )
        );

        using CancellationTokenSource lifetime = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        List<Task> connections = [];

        try
        {
            while (!lifetime.IsCancellationRequested)
            {
                TcpClient client = await listener
                    .AcceptTcpClientAsync(lifetime.Token)
                    .ConfigureAwait(continueOnCapturedContext: false);

                for (int index = connections.Count - 1; index >= 0; index--)
                {
                    if (connections[index].IsCompletedSuccessfully)
                        connections.RemoveAt(index);
                }

                connections.Add(HandleClientAsync(client, configuration, sessionLedger, lifetime.Token));
            }
        }
        catch (OperationCanceledException) when (lifetime.IsCancellationRequested)
        {
            ConnectionLog.Debug(logger, message: "Proxy listener stopped.");
        }
        finally
        {
            await lifetime.CancelAsync().ConfigureAwait(continueOnCapturedContext: false);
            listener.Stop();
            await Task.WhenAll(connections).ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private async Task HandleClientAsync(TcpClient socketClient, ProxyConfiguration configuration, ClientSessionLedger sessionLedger, CancellationToken cancellationToken)
    {
        using (socketClient)
        {
            try
            {
                await RunClientAsync(socketClient, configuration, sessionLedger, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (LoginException exception)
            {
                ConnectionLog.Warning(logger, $"Login rejected: {exception.Message}");
            }
            catch (Exception exception)
                when (ProxyFailureClassifier.IsRecoverable(exception))
            {
                if (!cancellationToken.IsCancellationRequested)
                    ConnectionLog.Write(logger, exception.Message);
            }
        }
    }

    private async Task RunClientAsync(
        TcpClient socketClient,
        ProxyConfiguration configuration,
        ClientSessionLedger sessionLedger,
        CancellationToken cancellationToken = default
    )
    {
        ConnectionLog.Write(
            logger,
            string.Create(CultureInfo.InvariantCulture, $"[{timeProvider.GetLocalNow():T}] Incoming connection from {socketClient.GetRemoteEndPoint()}")
        );

        ProxyCaptureWriter trafficCapture = new(configuration.CaptureDirectory, socketClient.GetRemoteEndPoint(), timeProvider);

        ConnectionLog.Write(
            logger,
            string.Create(CultureInfo.InvariantCulture, $"[{timeProvider.GetLocalNow():T}] Saving proxy traffic to {trafficCapture.DirectoryPath}")
        );

        ProxyConnection client = await ProxyConnection
            .ConnectAsync(
                socketClient,
                configuration.UpstreamHost,
                configuration.UpstreamPort,
                trafficCapture,
                sessionLedger,
                configuration.SessionAccountIdentifier,
                serverKeys,
                _commandDataResolver,
                logger,
                cancellationToken
            )
            .ConfigureAwait(continueOnCapturedContext: false);

        await using (client.ConfigureAwait(continueOnCapturedContext: false))
        {
            try
            {
                await client.RunAsync(client.CancellationTokenSource.Token).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (TaskCanceledException)
            {
                ConnectionLog.Debug(logger, message: "Proxy client stopped.");
            }
            finally
            {
                await client.CompletionTask.ConfigureAwait(continueOnCapturedContext: false);
            }
        }
    }
}
