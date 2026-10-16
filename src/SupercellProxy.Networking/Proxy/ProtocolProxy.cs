using System.Net;
using System.Net.Sockets;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SupercellProxy.Networking.Assets;
using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Captures;
using SupercellProxy.Networking.Cryptography;
using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Proxy;

/// <summary>
/// A man-in-the-middle game server: it accepts game clients on a local port, connects each one to the real server, decrypts and
/// re-encrypts the traffic in both directions, and can capture every frame to disk.
/// </summary>
/// <param name="options">Listen address and port, upstream server, capture and asset folders.</param>
/// <param name="serverKeys">Source of the real server's public key, needed to open the upstream connection.</param>
/// <param name="logger">Receives connection lifecycle and failure logs.</param>
/// <param name="timeProvider">Clock for capture timestamps.</param>
/// <param name="commandDataResolver">Game tables for decoding commands; loaded from the asset folder instead when one is configured.</param>
/// <param name="captureSession">
/// Optional hook started for each accepted client with its socket and capture writer. When present, the client's capture is only
/// published when the hook approves it. The returned object is disposed when the connection ends.
/// </param>
public sealed partial class ProtocolProxy(
    IOptions<ProxyOptions> options,
    IServerPublicKeySource serverKeys,
    ILogger<ProtocolProxy> logger,
    TimeProvider timeProvider,
    ICommandDataResolver? commandDataResolver = null,
    Func<TcpClient, ProxyCaptureWriter, ICommandDataResolver?, CancellationToken, ValueTask<IAsyncDisposable>>? captureSession = null
)
{
    private readonly TaskCompletionSource<IPEndPoint> _listening = new(TaskCreationOptions.RunContinuationsAsynchronously);

    private CaptureArchive? _archive;
    private ICommandDataResolver? _commandDataResolver = commandDataResolver;

    /// <summary>Completes with the actual endpoint when the listener starts, including an assigned ephemeral port.</summary>
    public Task<IPEndPoint> Listening => _listening.Task;

    /// <summary>
    /// Starts listening and serves clients until cancelled; each accepted client runs on its own connection in the background.
    /// </summary>
    /// <param name="cancellationToken">Stops accepting clients and ends the open connections.</param>
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        ProxyConfiguration configuration = options.Value.ToConfiguration();
        _archive = configuration.CaptureDirectory is { } captureRoot ? new CaptureArchive(captureRoot, configuration.AssetDirectory) : null;

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

        await RunConnectionsAsync(listener, configuration, lifetime).ConfigureAwait(continueOnCapturedContext: false);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Saving versioned proxy recordings under {CaptureDirectory}")]
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

    [LoggerMessage(Level = LogLevel.Warning, Message = "Upstream login rejected: {Reason}")]
    private static partial void LogLoginRejected(ILogger logger, string reason);

    private async Task HandleClientAsync(TcpClient socketClient, ProxyConfiguration configuration, CancellationToken cancellationToken)
    {
        string remoteEndPoint = socketClient.GetRemoteEndPoint();

        using (socketClient)
        {
            try
            {
                await RunClientAsync(socketClient, configuration, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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

    private async Task RunClientAsync(TcpClient socketClient, ProxyConfiguration configuration, CancellationToken cancellationToken = default)
    {
        string remoteEndPoint = socketClient.GetRemoteEndPoint();
        LogIncomingConnection(logger, remoteEndPoint);

        ProxyCaptureWriter trafficCapture = new(configuration.CaptureDirectory, remoteEndPoint, timeProvider)
        {
            DeferPublication = captureSession is not null,
            Archive = _archive,
        };

        if (configuration.CaptureDirectory is { } archiveRoot) LogCaptureDirectory(logger, archiveRoot);

        IAsyncDisposable? recording = captureSession is null
            ? null
            : await captureSession(socketClient, trafficCapture, _commandDataResolver, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        try
        {
            ProxyConnection client = await ProxyConnection
                .ConnectAsync(
                    socketClient,
                    configuration.UpstreamHost,
                    configuration.UpstreamPort,
                    trafficCapture,
                    configuration.SessionTokenProvider,
                    serverKeys,
                    _commandDataResolver,
                    logger,
                    configuration.AnonymousAccount,
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
        finally
        {
            if (recording is not null)
                await recording.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private async Task RunConnectionsAsync(TcpListener listener, ProxyConfiguration configuration, CancellationTokenSource lifetime)
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

                connections.Add(HandleClientAsync(client, configuration, lifetime.Token));
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
