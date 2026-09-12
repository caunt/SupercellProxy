using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Client;

internal sealed class ClientHostedService(
    ProtocolClientFactory clients,
    IOptions<ClientOptions> options,
    Func<IMessage, CancellationToken, Task> onMessage,
    IHostApplicationLifetime lifetime
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ProtocolClient client = clients.Create(options.Value.ToConfiguration());

        await using (client.ConfigureAwait(continueOnCapturedContext: false))
            await client.RunAsync(onMessage, stoppingToken).ConfigureAwait(continueOnCapturedContext: false);

        lifetime.StopApplication();
    }
}
