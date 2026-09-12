using Microsoft.Extensions.Hosting;

namespace SupercellProxy.Networking.Proxy;

internal sealed class ProxyHostedService(ProtocolProxy proxy) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return proxy.RunAsync(stoppingToken);
    }
}
