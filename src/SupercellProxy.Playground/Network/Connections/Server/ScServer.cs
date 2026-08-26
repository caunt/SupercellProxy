using System.Globalization;

namespace SupercellProxy.Playground.Network.Connections.Server;

/// <summary>
/// Hosts the entry point for a future local server.
/// Protocol handling is not implemented yet.
/// </summary>
public sealed class ScServer(string listenAddress, int listenPort)
{
    private readonly string listenAddress = listenAddress;
    private readonly int listenPort = listenPort;

    /// <summary>
    /// Reports the configured endpoint.
    /// Does not accept connections.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Console.WriteLine(
            string.Create(
                CultureInfo.InvariantCulture,
                $"Server placeholder configured for {listenAddress}:{listenPort}; protocol handling is not implemented."
            )
        );
        await Task.CompletedTask.ConfigureAwait(false);
    }
}
