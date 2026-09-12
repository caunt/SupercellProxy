using System.Globalization;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SupercellProxy.Networking.Hosting;

namespace SupercellProxy.Networking.Server;

/// <summary>
/// Hosts the entry point for a future local server.
/// Protocol handling is not implemented yet.
/// </summary>
public sealed class ServerPlaceholder(IOptions<ServerOptions> options, ILogger<ServerPlaceholder> logger)
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
        ConnectionLog.Write(
            logger,
            string.Create(
                CultureInfo.InvariantCulture,
                $"Server placeholder configured for {_listenAddress}:{_listenPort}; protocol handling is not implemented."
            )
        );
        await Task.CompletedTask.ConfigureAwait(continueOnCapturedContext: false);
    }
}
