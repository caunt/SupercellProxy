using System.CommandLine;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Hosting;
using SupercellProxy.Networking.Proxy;

namespace SupercellProxy.Capture;

internal static class CaptureCommandLine
{
    private static readonly Option<string?> AccountOption = new(name: "--account", aliases: ["-a"])
    {
        Arity = ArgumentArity.ZeroOrOne,
        Description = "Select a saved session by an optional farm-name fragment",
    };

    private static readonly Option<string?> AssetDirectoryOption = new(name: "--asset-directory", aliases: ["-d"]) { Description = "Directory containing versioned game assets" };
    private static readonly Option<string?> CaptureDirectoryOption = new(name: "--capture-directory", aliases: ["-c"]) { Description = "Directory containing retained and newly recorded captures" };
    private static readonly Option<int?> KeyVersionOption = new(name: "--key-version") { Description = "Protocol key version" };
    private static readonly Option<string?> LedgerOption = new(name: "--ledger", aliases: ["-l"]) { Description = "Client session ledger path" };
    private static readonly Option<string?> ListenAddressOption = new(name: "--listen-address", aliases: ["-b"]) { Description = "Local listener address" };
    private static readonly Option<int?> ListenPortOption = new(name: "--listen-port", aliases: ["-p"]) { Description = "Local listener port" };
    private static readonly Option<int?> MajorVersionOption = new(name: "--major-version") { Description = "Protocol major application version" };
    private static readonly Option<int?> MinorVersionOption = new(name: "--minor-version") { Description = "Protocol minor application version" };
    private static readonly Option<int?> PatchVersionOption = new(name: "--patch-version") { Description = "Protocol patch application version" };
    private static readonly Option<int?> ProtocolVersionOption = new(name: "--protocol-version") { Description = "Protocol transport version" };
    private static readonly Option<string?> UpstreamHostOption = new(name: "--upstream-host", aliases: ["-u"]) { Description = "Upstream game server host" };
    private static readonly Option<int?> UpstreamPortOption = new(name: "--upstream-port") { Description = "Upstream game server port" };

    internal static async Task<int> InvokeAsync(string[] arguments)
    {
        RootCommand command = CreateCommand();

        return await command.Parse(arguments).InvokeAsync().ConfigureAwait(continueOnCapturedContext: false);
    }

    private static void ApplyCommandLine(ProxyOptions options, ParseResult parseResult)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (parseResult.GetResult(AssetDirectoryOption) is not null)
            options.AssetDirectory = parseResult.GetValue(AssetDirectoryOption);

        if (parseResult.GetResult(CaptureDirectoryOption) is not null)
            options.CaptureDirectory = parseResult.GetValue(CaptureDirectoryOption);

        if (parseResult.GetValue(KeyVersionOption) is { } keyVersion)
            options.Protocol = options.Protocol with { KeyVersion = keyVersion };

        if (parseResult.GetResult(LedgerOption) is not null)
            options.SessionLedgerPath = parseResult.GetValue(LedgerOption);

        if (parseResult.GetResult(ListenAddressOption) is not null)
            options.ListenAddress = parseResult.GetValue(ListenAddressOption) ?? string.Empty;

        if (parseResult.GetValue(ListenPortOption) is { } listenPort)
            options.ListenPort = listenPort;

        if (parseResult.GetValue(MajorVersionOption) is { } majorVersion)
            options.Protocol = options.Protocol with { MajorVersion = majorVersion };

        if (parseResult.GetValue(MinorVersionOption) is { } minorVersion)
            options.Protocol = options.Protocol with { MinorVersion = minorVersion };

        if (parseResult.GetValue(PatchVersionOption) is { } patchVersion)
            options.Protocol = options.Protocol with { PatchVersion = patchVersion };

        if (parseResult.GetValue(ProtocolVersionOption) is { } protocolVersion)
            options.Protocol = options.Protocol with { ProtocolVersion = protocolVersion };

        if (parseResult.GetResult(UpstreamHostOption) is not null)
            options.UpstreamHost = parseResult.GetValue(UpstreamHostOption) ?? string.Empty;

        if (parseResult.GetValue(UpstreamPortOption) is { } upstreamPort)
            options.UpstreamPort = upstreamPort;
    }

    private static RootCommand CreateCommand()
    {
        RootCommand command = new(description: "Capture and proxy Supercell protocol traffic");
        command.Options.Add(AccountOption);
        command.Options.Add(AssetDirectoryOption);
        command.Options.Add(CaptureDirectoryOption);
        command.Options.Add(KeyVersionOption);
        command.Options.Add(LedgerOption);
        command.Options.Add(ListenAddressOption);
        command.Options.Add(ListenPortOption);
        command.Options.Add(MajorVersionOption);
        command.Options.Add(MinorVersionOption);
        command.Options.Add(PatchVersionOption);
        command.Options.Add(ProtocolVersionOption);
        command.Options.Add(UpstreamHostOption);
        command.Options.Add(UpstreamPortOption);
        command.SetAction(RunAsync);

        return command;
    }

    private static async Task<int> RunAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        try
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();

            CaptureAccountSelection selection = new(Specified: parseResult.GetResult(AccountOption) is not null, FarmNameFragment: parseResult.GetValue(AccountOption));

            OptionsBuilder<ProxyOptions> proxyOptions = builder.Services
                .AddSingleton(selection)
                .AddSingleton<IHostedService>(
                    static provider => new CaptureSessionImportService(
                        provider.GetRequiredService<IOptions<ProxyOptions>>(),
                        provider.GetRequiredService<ProtocolClientFactory>(),
                        provider.GetRequiredService<CaptureAccountSelection>(),
                        provider.GetRequiredService<ILogger<CaptureSessionImportService>>()
                    )
                )
                .AddProtocolProxy(static options => options.CaptureDirectory = ProxyCaptureWriter.RootDirectoryPath)
                .Bind(builder.Configuration)
                .Configure(options => ApplyCommandLine(options, parseResult));

            await builder.Build().RunAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            return 0;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return 130;
        }
        catch (Exception exception) when (exception is not OutOfMemoryException)
        {
            await Console.Error.WriteLineAsync($"Error: {exception.Message}").ConfigureAwait(continueOnCapturedContext: false);

            return 1;
        }
    }
}
