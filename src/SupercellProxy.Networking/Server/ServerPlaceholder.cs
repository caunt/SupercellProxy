using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SupercellProxy.Networking.Server;

/// <summary>
/// Hosts the entry point for a future local server.
/// Protocol handling is not implemented yet.
/// </summary>
public sealed partial class ServerPlaceholder(IOptions<ServerOptions> options, ILogger<ServerPlaceholder> logger)
{
    private readonly string _listenAddress = options.Value.ListenAddress;
    private readonly int _listenPort = options.Value.ListenPort;

    /// <summary>
    /// Reports the configured endpoint.
    /// Does not accept connections.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LogConfigured(logger, _listenAddress, _listenPort);

        await Task.CompletedTask.ConfigureAwait(continueOnCapturedContext: false);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Server placeholder configured for {ListenAddress}:{ListenPort}; protocol handling is not implemented.")]
    private static partial void LogConfigured(ILogger logger, string listenAddress, int listenPort);
}
