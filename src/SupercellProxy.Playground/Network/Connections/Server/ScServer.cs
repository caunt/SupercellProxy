using System.Globalization;
using SupercellProxy.Playground.Network.Configuration;

namespace SupercellProxy.Playground.Network.Connections.Server;

/// <summary>
/// Hosts the entry point for a future local server.
/// Protocol handling is not implemented yet.
/// </summary>
internal sealed class ScServer(string listenAddress, int listenPort)
{
    private readonly string _listenAddress = listenAddress;
    private readonly int _listenPort = listenPort;

    /// Creates the server placeholder from command-line listen arguments.
    public static ScServer Create(string[] arguments)
    {
        return new ScServer(
            arguments.ElementAtOrDefault(0) ?? ConnectionAddress.DefaultListenHost,
            ConnectionAddress.ParsePort(arguments.ElementAtOrDefault(1))
        );
    }

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
                $"Server placeholder configured for {_listenAddress}:{_listenPort}; protocol handling is not implemented."
            )
        );
        await Task.CompletedTask.ConfigureAwait(false);
    }
}
