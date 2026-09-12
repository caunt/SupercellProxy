using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using SupercellProxy.Networking.Hosting;
using SupercellProxy.Networking.Proxy;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

OptionsBuilder<ProxyOptions> proxyOptions = builder
    .Services.AddProtocolProxy(static options => options.CaptureDirectory = ProxyCaptureWriter.RootDirectoryPath)
    .Bind(builder.Configuration);

await builder.Build().RunAsync().ConfigureAwait(continueOnCapturedContext: false);
