using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using DnsClient;
using SupercellProxy.Playground;
using SupercellProxy.Playground.Network.Configuration;
using SupercellProxy.Playground.Network.Connections.Client;
using SupercellProxy.Playground.Network.Connections.Proxy;
using SupercellProxy.Playground.Network.Connections.Server;

var cancellationToken = CancellationToken.None;
var (runMode, connectionArguments) = ResolveRunMode(args);

switch (runMode)
{
    case RunMode.Client:
        await RunClientAsync(connectionArguments, cancellationToken).ConfigureAwait(false);
        break;
    case RunMode.Proxy:
        await RunProxyAsync(connectionArguments, cancellationToken).ConfigureAwait(false);
        break;
    case RunMode.Server:
        await RunServerAsync(connectionArguments, cancellationToken).ConfigureAwait(false);
        break;
    default:
        throw new InvalidOperationException($"Unsupported run mode: {runMode}.");
}

static (RunMode Mode, string[] Arguments) ResolveRunMode(string[] arguments)
{
    if (
        !Enum.TryParse<RunMode>(arguments.ElementAtOrDefault(0), ignoreCase: true, out var mode)
        || !Enum.IsDefined(mode)
    )
        return (RunMode.Proxy, arguments);

    return (mode, arguments[1..]);
}

static async Task RunClientAsync(string[] arguments, CancellationToken cancellationToken)
{
    var (upstreamHost, upstreamPort) = await ResolveUpstreamAsync(arguments, cancellationToken)
        .ConfigureAwait(false);
    var client = new ScClient(
        new ClientConfiguration(upstreamHost, upstreamPort, CreateProtocolConfiguration())
    );
    await using (client.ConfigureAwait(false))
        await client.RunAsync(cancellationToken).ConfigureAwait(false);
}

static async Task RunProxyAsync(string[] arguments, CancellationToken cancellationToken)
{
    var (upstreamHost, upstreamPort) = await ResolveUpstreamAsync(arguments, cancellationToken)
        .ConfigureAwait(false);
    var listenAddress = arguments.ElementAtOrDefault(2) ?? "0.0.0.0";
    var listenPort = ParsePort(arguments.ElementAtOrDefault(3));
    var proxy = new ScProxy(
        new ProxyConfiguration(
            upstreamHost,
            upstreamPort,
            listenAddress,
            listenPort,
            CreateProtocolConfiguration()
        )
    );
    await proxy.RunAsync(cancellationToken).ConfigureAwait(false);
}

static async Task RunServerAsync(string[] arguments, CancellationToken cancellationToken)
{
    var listenAddress = arguments.ElementAtOrDefault(0) ?? "0.0.0.0";
    var listenPort = ParsePort(arguments.ElementAtOrDefault(1));
    var server = new ScServer(listenAddress, listenPort);
    await server.RunAsync(cancellationToken).ConfigureAwait(false);
}

static ProtocolConfiguration CreateProtocolConfiguration() => new(1, 72, 86, 3, 43);

static int ParsePort(string? value)
{
    return int.TryParse(value, CultureInfo.InvariantCulture, out var port) ? port : 9339;
}

static async Task<(string Host, int Port)> ResolveUpstreamAsync(
    string[] arguments,
    CancellationToken cancellationToken
)
{
    var host = arguments.ElementAtOrDefault(0) ?? "game.haydaygame.com";
    var resolvedHost = await ResolveHostAsync(host, cancellationToken).ConfigureAwait(false);
    return (resolvedHost, ParsePort(arguments.ElementAtOrDefault(1)));
}

static async Task<string> ResolveHostAsync(string host, CancellationToken cancellationToken)
{
    if (IPAddress.IsValid(host))
        return host;

    var resolvedAddress = await TryResolveWithDnsAsync(host, cancellationToken)
        .ConfigureAwait(false);
    return resolvedAddress
        ?? await ResolveWithDnsOverHttpsAsync(host, cancellationToken).ConfigureAwait(false);
}

static async Task<string?> TryResolveWithDnsAsync(string host, CancellationToken cancellationToken)
{
    try
    {
        var lookupClient = new LookupClient(
            new LookupClientOptions(IPAddress.Parse("1.1.1.1"))
            {
                Timeout = TimeSpan.FromSeconds(2),
                Retries = 0,
            }
        );
        var response = await lookupClient
            .QueryAsync(host, QueryType.A, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        return response.Answers.ARecords().FirstOrDefault()?.Address.ToString();
    }
    catch (Exception exception)
        when (exception
                is DnsResponseException
                    or OperationCanceledException
                    or SocketException
                    or TimeoutException
        )
    {
        return null;
    }
}

static async Task<string> ResolveWithDnsOverHttpsAsync(
    string host,
    CancellationToken cancellationToken
)
{
    using var httpClient = new HttpClient();
    using var request = new HttpRequestMessage(
        HttpMethod.Get,
        $"https://1.1.1.1/dns-query?name={Uri.EscapeDataString(host)}&type=A"
    );
    request.Headers.Accept.ParseAdd("application/dns-json");

    using var response = await httpClient
        .SendAsync(request, cancellationToken)
        .ConfigureAwait(false);
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
    await using (content.ConfigureAwait(false))
    {
        using var document = await JsonDocument
            .ParseAsync(content, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        return document
            .RootElement.GetProperty("Answer")
            .EnumerateArray()
            .Select(static answer => answer.GetProperty("data").GetString())
            .First(static address =>
                IPAddress.TryParse(address, out var parsed)
                && parsed.AddressFamily is AddressFamily.InterNetwork
            )!;
    }
}
