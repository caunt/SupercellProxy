using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SupercellProxy.Capture;
using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Hosting;
using SupercellProxy.Networking.Proxy;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

OptionsBuilder<ProxyOptions> proxyOptions = builder
    .Services.AddSingleton<IHostedService>(
        static provider => new CaptureSessionImportService(
            provider.GetRequiredService<IOptions<ProxyOptions>>(),
            provider.GetRequiredService<ProtocolClientFactory>(),
            provider.GetRequiredService<ILogger<CaptureSessionImportService>>()
        )
    )
    .AddProtocolProxy(static options => options.CaptureDirectory = ProxyCaptureWriter.RootDirectoryPath)
    .Bind(builder.Configuration);

await builder.Build().RunAsync().ConfigureAwait(continueOnCapturedContext: false);
