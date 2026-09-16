using System.Net;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Cryptography;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Proxy;

namespace SupercellProxy.Networking.Hosting;

/// <summary>Registers public connection infrastructure independently of game simulation.</summary>
public static class NetworkingServiceCollectionExtensions
{

    /// <summary>Registers an authenticated hosted client and returns its typed options.</summary>
    public static OptionsBuilder<ClientOptions> AddProtocolClient(this IServiceCollection services, Func<IMessage, CancellationToken, Task> onMessage, Action<ClientOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(onMessage);

        OptionsBuilder<ClientOptions> options = services.AddSupercellNetworking()
            .AddOptions<ClientOptions>()
            .Validate(static value => !string.IsNullOrWhiteSpace(value.UpstreamHost), $"{nameof(ClientOptions.UpstreamHost)} is required.")
            .Validate(
                static value => value.UpstreamPort is > 0 and <= IPEndPoint.MaxPort,
                $"{nameof(ClientOptions.UpstreamPort)} must be between 1 and {IPEndPoint.MaxPort}."
            )
            .Validate(static value => value.Protocol is not null, $"{nameof(ClientOptions.Protocol)} is required.")
            .Validate(static value => value.SessionTokenProvider is not null, failureMessage: "A session-token provider is required.")
            .ValidateOnStart();

        if (configure is not null)
            options = options.Configure(configure);

        services.Add(ServiceDescriptor.Singleton(onMessage));
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IHostedService, ClientHostedService>(
                provider => new ClientHostedService(
            provider.GetRequiredService<ProtocolClientFactory>(),
            provider.GetRequiredService<IOptions<ClientOptions>>(),
            onMessage,
            provider.GetRequiredService<IHostApplicationLifetime>()
        )
            )
        );

        return options;
    }

    /// <summary>Registers a hosted proxy and returns its typed options for binding or configuration.</summary>
    public static OptionsBuilder<ProxyOptions> AddProtocolProxy(this IServiceCollection services, Action<ProxyOptions>? configure = null)
    {
        OptionsBuilder<ProxyOptions> options = services.AddSupercellNetworking()
            .AddOptions<ProxyOptions>()
            .Validate(static value => !string.IsNullOrWhiteSpace(value.UpstreamHost), $"{nameof(ProxyOptions.UpstreamHost)} is required.")
            .Validate(
                static value => value.UpstreamPort is > 0 and <= IPEndPoint.MaxPort,
                $"{nameof(ProxyOptions.UpstreamPort)} must be between 1 and {IPEndPoint.MaxPort}."
            )
            .Validate(static value => IPAddress.TryParse(value.ListenAddress, out _), $"{nameof(ProxyOptions.ListenAddress)} must be an IP address.")
            .Validate(
                static value => value.ListenPort is >= 0 and <= IPEndPoint.MaxPort,
                $"{nameof(ProxyOptions.ListenPort)} must be between 0 and {IPEndPoint.MaxPort}."
            )
            .Validate(static value => value.Protocol is not null, $"{nameof(ProxyOptions.Protocol)} is required.")
            .ValidateOnStart();

        if (configure is not null)
            options = options.Configure(configure);

        services.Add(ServiceDescriptor.Singleton<ProtocolProxy, ProtocolProxy>());
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IHostedService, ProxyHostedService>(static provider => new ProxyHostedService(provider.GetRequiredService<ProtocolProxy>()))
        );

        return options;
    }

    /// <summary>Registers factories, HTTP, logging, and replaceable system time.</summary>
    public static IServiceCollection AddSupercellNetworking(this IServiceCollection services)
    {
        services = services
            .AddLogging(
                static logging =>
                {
                    if (logging.AddFilter(category: "System.Net.Http.HttpClient", LogLevel.Warning) is null)
                        throw new InvalidOperationException(message: "Failed to configure HTTP client logging.");
                }
            )
            .AddHttpClient()
            .AddHttpClient(name: "ServerKeys")
            .AddTypedClient<IServerPublicKeySource>(static web => new HayDayServerPublicKeySource(web))
            .Services;
        services.TryAddSingleton(TimeProvider.System);
        services.TryAddSingleton<ProtocolClientFactory>();

        return services;
    }

}
