using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using DnsClient;

namespace SupercellProxy.Playground.Network.Configuration;

internal static class ConnectionAddress
{
    internal const string DefaultListenHost = "0.0.0.0";
    internal const int DefaultPort = 9339;
    internal const string DefaultUpstreamHost = "game.haydaygame.com";

    public static async Task<(string Host, int Port)> ResolveAsync(
        string[] arguments,
        CancellationToken cancellationToken
    )
    {
        var host = arguments.ElementAtOrDefault(0) ?? DefaultUpstreamHost;
        var resolvedHost = await ResolveHostAsync(host, cancellationToken).ConfigureAwait(false);
        return (resolvedHost, ParsePort(arguments.ElementAtOrDefault(1)));
    }

    public static int ParsePort(string? value)
    {
        return int.TryParse(value, CultureInfo.InvariantCulture, out var port) ? port : DefaultPort;
    }

    private static async Task<string> ResolveHostAsync(
        string host,
        CancellationToken cancellationToken
    )
    {
        if (IPAddress.IsValid(host))
            return host;

        var resolvedAddress = await TryResolveWithDnsAsync(host, cancellationToken)
            .ConfigureAwait(false);
        return resolvedAddress
            ?? await ResolveWithDnsOverHttpsAsync(host, cancellationToken).ConfigureAwait(false);
    }

    private static async Task<string?> TryResolveWithDnsAsync(
        string host,
        CancellationToken cancellationToken
    )
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

    private static async Task<string> ResolveWithDnsOverHttpsAsync(
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
        var content = await response
            .Content.ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);
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
                    )
                ?? throw new InvalidDataException(
                    $"DNS-over-HTTPS returned no IPv4 address for {host}."
                );
        }
    }
}
