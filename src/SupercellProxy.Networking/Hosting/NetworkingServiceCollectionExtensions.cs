using System.Net;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Cryptography;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Proxy;
using SupercellProxy.Networking.Server;

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
            .Validate(
                static value => LongIdentifier.TryParse(value.SessionAccountIdentifier, out LongIdentifier accountIdentifier)
                    && accountIdentifier != LongIdentifier.Empty,
                $"{nameof(ClientOptions.SessionAccountIdentifier)} must be a valid nonempty account tag."
            )
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
            .Validate(
                static value => value.SessionAccountIdentifier is null
                    || (LongIdentifier.TryParse(value.SessionAccountIdentifier, out LongIdentifier accountIdentifier)
                        && accountIdentifier != LongIdentifier.Empty),
                $"{nameof(ProxyOptions.SessionAccountIdentifier)} must be null or a valid nonempty account tag."
            )
            .ValidateOnStart();

        if (configure is not null)
            options = options.Configure(configure);

        services.Add(ServiceDescriptor.Singleton<ProtocolProxy, ProtocolProxy>());
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IHostedService, ProxyHostedService>(static provider => new ProxyHostedService(provider.GetRequiredService<ProtocolProxy>()))
        );

        return options;
    }

    /// <summary>Registers the server placeholder and returns its typed endpoint options.</summary>
    public static OptionsBuilder<ServerOptions> AddServerPlaceholder(this IServiceCollection services, Action<ServerOptions>? configure = null)
    {
        OptionsBuilder<ServerOptions> options = CreateServerOptions(services);

        if (configure is not null)
            options = options.Configure(configure);

        RegisterServerWorker(options.Services);

        return options;
    }

    /// <summary>Registers the server host and binds its typed options to configuration.</summary>
    public static void AddServerPlaceholder(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        OptionsBuilder<ServerOptions> options = CreateServerOptions(services).Bind(configuration);
        RegisterServerWorker(options.Services);
    }

    /// <summary>Registers factories, HTTP, logging, and replaceable system time.</summary>
    public static IServiceCollection AddSupercellNetworking(this IServiceCollection services)
    {
        services = services.AddLogging().AddHttpClient()
            .AddHttpClient(name: "ServerKeys")
            .AddTypedClient<IServerPublicKeySource>(static web => new HayDayServerPublicKeySource(web))
            .Services;
        services.TryAddSingleton(TimeProvider.System);
        services.TryAddSingleton<ProtocolClientFactory>();

        return services;
    }

    private static OptionsBuilder<ServerOptions> CreateServerOptions(IServiceCollection services)
    {
        return services.AddSupercellNetworking()
            .AddOptions<ServerOptions>()
            .Validate(static value => IPAddress.TryParse(value.ListenAddress, out _), $"{nameof(ServerOptions.ListenAddress)} must be an IP address.")
            .Validate(
                static value => value.ListenPort is >= 0 and <= IPEndPoint.MaxPort,
                $"{nameof(ServerOptions.ListenPort)} must be between 0 and {IPEndPoint.MaxPort}."
            )
            .ValidateOnStart();
    }

    private static void RegisterServerWorker(IServiceCollection services)
    {
        services.Add(ServiceDescriptor.Singleton<ServerPlaceholder, ServerPlaceholder>());
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IHostedService, ServerHostedService>(
                static provider => new ServerHostedService(provider.GetRequiredService<ServerPlaceholder>(), provider.GetRequiredService<IHostApplicationLifetime>())
            )
        );

    }
}
