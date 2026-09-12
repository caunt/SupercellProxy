using System.Net;
using System.Net.Sockets;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SupercellProxy.Networking.Assets;
using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Cryptography;
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
public sealed partial class ProtocolProxy(
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
            LogListeningEndpointAlreadyReported(logger);

        LogListening(logger, configuration.ListenAddress, configuration.ListenPort, configuration.UpstreamHost, configuration.UpstreamPort);

        using CancellationTokenSource lifetime = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        await RunConnectionsAsync(listener, configuration, sessionLedger, lifetime).ConfigureAwait(continueOnCapturedContext: false);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Saving proxy traffic to {CaptureDirectory}")]
    private static partial void LogCaptureDirectory(ILogger logger, string captureDirectory);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Proxy client stopped.")]
    private static partial void LogClientStopped(ILogger logger);

    [LoggerMessage(Level = LogLevel.Error, Message = "Connection {RemoteEndPoint} failed")]
    private static partial void LogConnectionFailed(ILogger logger, string remoteEndPoint, Exception exception);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Connection {RemoteEndPoint} stopped.")]
    private static partial void LogConnectionStopped(ILogger logger, string remoteEndPoint);

    [LoggerMessage(Level = LogLevel.Information, Message = "Incoming connection from {RemoteEndPoint}")]
    private static partial void LogIncomingConnection(ILogger logger, string remoteEndPoint);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Proxy listener stopped.")]
    private static partial void LogListenerStopped(ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Listening on {ListenAddress}:{ListenPort}, upstream {UpstreamHost}:{UpstreamPort}")]
    private static partial void LogListening(ILogger logger, string listenAddress, int listenPort, string upstreamHost, int upstreamPort);

    [LoggerMessage(Level = LogLevel.Debug, Message = "The proxy listening endpoint was already reported.")]
    private static partial void LogListeningEndpointAlreadyReported(ILogger logger);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Login rejected: {Reason}")]
    private static partial void LogLoginRejected(ILogger logger, string reason);

    private async Task HandleClientAsync(TcpClient socketClient, ProxyConfiguration configuration, ClientSessionLedger sessionLedger, CancellationToken cancellationToken)
    {
        string remoteEndPoint = socketClient.GetRemoteEndPoint();

        using (socketClient)
        {
            try
            {
                await RunClientAsync(socketClient, configuration, sessionLedger, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                LogConnectionStopped(logger, remoteEndPoint);
            }
            catch (LoginException exception)
            {
                LogLoginRejected(logger, exception.Message);
            }
            catch (Exception exception) when (exception is not OutOfMemoryException)
            {
                LogConnectionFailed(logger, remoteEndPoint, exception);
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
        string remoteEndPoint = socketClient.GetRemoteEndPoint();
        LogIncomingConnection(logger, remoteEndPoint);

        ProxyCaptureWriter trafficCapture = new(configuration.CaptureDirectory, remoteEndPoint, timeProvider);
        LogCaptureDirectory(logger, trafficCapture.DirectoryPath);

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
                LogClientStopped(logger);
            }
            finally
            {
                await client.CompletionTask.ConfigureAwait(continueOnCapturedContext: false);
            }
        }
    }

    private async Task RunConnectionsAsync(TcpListener listener, ProxyConfiguration configuration, ClientSessionLedger sessionLedger, CancellationTokenSource lifetime)
    {
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
            LogListenerStopped(logger);
        }
        finally
        {
            await lifetime.CancelAsync().ConfigureAwait(continueOnCapturedContext: false);
            listener.Stop();
            await Task.WhenAll(connections).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
