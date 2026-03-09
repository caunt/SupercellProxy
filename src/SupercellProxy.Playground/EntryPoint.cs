using DnsClient;
using SupercellProxy.Playground.Network.Sides.Configuration;
using SupercellProxy.Playground.Network.Sides.Proxy;
using System.Net;

var upstreamHost = args.Length > 0 ? args[0] : "game.haydaygame.com";
var upstreamPort = args.Length > 1 && int.TryParse(args[1], out var up) ? up : 9339;

if (!IPAddress.IsValid(upstreamHost))
{
    var lookupClient = new LookupClient(new IPEndPoint(IPAddress.Parse("1.1.1.1"), 53));

    var dnsQueryResponse = await lookupClient.QueryAsync(upstreamHost, QueryType.A);
    upstreamHost = dnsQueryResponse.Answers.ARecords().First().Address.ToString();
}

// var client = new ScClient(new ClientConfiguration(
//     UpstreamHost: upstreamHost,
//     UpstreamPort: upstreamPort,
//     Protocol: new ProtocolConfiguration(
//         MajorVersion: 1,
//         MinorVersion: 69,
//         PatchVersion: 89,
//         ProtocolVersion: 3,
//         KeyVersion: 40)));
// 
// await client.RunAsync();

var listenAddress = args.Length > 2 ? args[2] : "0.0.0.0";
var listenPort = args.Length > 3 && int.TryParse(args[3], out var lp) ? lp : 9339;

var proxy = new ScProxy(new ProxyConfiguration(
    UpstreamHost: upstreamHost,
    UpstreamPort: upstreamPort,
    ListenAddress: listenAddress,
    ListenPort: listenPort,
    Protocol: new ProtocolConfiguration(
        MajorVersion: 1,
        MinorVersion: 69,
        PatchVersion: 89,
        ProtocolVersion: 3,
        KeyVersion: 40)));

await proxy.RunAsync();

// var server = new ScServer(listenAddress, listenPort);
// await server.RunAsync();