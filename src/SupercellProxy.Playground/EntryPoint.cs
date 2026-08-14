using DnsClient;
using SupercellProxy.Playground.Network.Sides;
using SupercellProxy.Playground.Network.Sides.Configuration;
using SupercellProxy.Playground.Network.Sides.Proxy;
using SupercellProxy.Playground.Supercell;
using System.Net;
using System.Net.Sockets;

var runClient = args.FirstOrDefault() is "client";
var captureOtherFishingHome = runClient && args.ElementAtOrDefault(1) is "visit";
var captureTarget = captureOtherFishingHome
    ? LogicLong.Parse(args.ElementAtOrDefault(2) ?? throw new ArgumentException("A target player tag is required."))
    : (LogicLong?)null;
var capturePath = captureTarget is { } capturedTarget
    ? args.ElementAtOrDefault(3) ?? Path.Combine("bin", $"other-fishing-home-{capturedTarget.ToFormattedString(includeHashPrefix: false)}.json")
    : null;
var connectionArguments = runClient
    ? args[(captureOtherFishingHome ? Math.Min(args.Length, 4) : 1)..]
    : args;
var upstreamHost = connectionArguments.Length > 0
    ? connectionArguments[0]
    : "game.haydaygame.com";
var upstreamPort = connectionArguments.Length > 1 &&
                   int.TryParse(connectionArguments[1], out var up)
    ? up
    : 9339;

upstreamHost = await ResolveHostAsync(upstreamHost);

var protocol = new ProtocolConfiguration(
    MajorVersion: 1,
    MinorVersion: 72,
    PatchVersion: 84,
    ProtocolVersion: 3,
    KeyVersion: 43);

if (runClient)
{
    await using var client = new ScClient(new ClientConfiguration(
        UpstreamHost: upstreamHost,
        UpstreamPort: upstreamPort,
        Protocol: protocol));

    if (captureTarget is { } target)
        await client.CaptureOtherFishingHomeAsync(target, capturePath!);
    else
        await client.RunAsync();
    return;
}

var listenAddress = connectionArguments.Length > 2 ? connectionArguments[2] : "0.0.0.0";
var listenPort = connectionArguments.Length > 3 &&
                 int.TryParse(connectionArguments[3], out var lp)
    ? lp
    : 9339;

var proxy = new ScProxy(new ProxyConfiguration(
    UpstreamHost: upstreamHost,
    UpstreamPort: upstreamPort,
    ListenAddress: listenAddress,
    ListenPort: listenPort,
    Protocol: protocol));

await proxy.RunAsync();

// var server = new ScServer(listenAddress, listenPort);
// await server.RunAsync();

static async Task<string> ResolveHostAsync(string host)
{
    if (IPAddress.IsValid(host))
        return host;

    try
    {
        var response = await new LookupClient().QueryAsync(host, QueryType.A);
        var address = response.Answers.ARecords().FirstOrDefault()?.Address;

        if (address is not null)
            return address.ToString();
    }
    catch (Exception exception) when (exception is DnsResponseException or
                                                   OperationCanceledException or
                                                   SocketException)
    {
        // Fall back to the system resolver below.
    }

    var addresses = await Dns.GetHostAddressesAsync(host);
    return addresses.First(address => address.AddressFamily is AddressFamily.InterNetwork)
        .ToString();
}
