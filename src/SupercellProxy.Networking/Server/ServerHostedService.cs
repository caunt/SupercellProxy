using Microsoft.Extensions.Hosting;

namespace SupercellProxy.Networking.Server;

internal sealed class ServerHostedService(ServerPlaceholder server, IHostApplicationLifetime lifetime)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await server.RunAsync(stoppingToken).ConfigureAwait(continueOnCapturedContext: false);
        lifetime.StopApplication();
    }
}
