using Microsoft.Extensions.Hosting;

using SupercellProxy.Networking.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddServerPlaceholder(builder.Configuration);
await builder.Build().RunAsync().ConfigureAwait(continueOnCapturedContext: false);
