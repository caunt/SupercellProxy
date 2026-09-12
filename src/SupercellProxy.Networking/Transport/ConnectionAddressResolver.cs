using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

using DnsClient;

namespace SupercellProxy.Networking.Transport;

/// <summary>
/// Defines the Connection Address contract.
/// </summary>
public static class ConnectionAddressResolver
{
    /// <summary>
    /// Provides the Default Listen Host value or operation.
    /// </summary>
    public const string DefaultListenHost = "0.0.0.0";

    /// <summary>
    /// Provides the Default Port value or operation.
    /// </summary>
    public const int DefaultPort = 9339;

    /// <summary>
    /// Provides the Default Upstream Host value or operation.
    /// </summary>
    public const string DefaultUpstreamHost = "game.haydaygame.com";

    /// <summary>
    /// Provides the Parse Port value or operation.
    /// </summary>
    public static int ParsePort(string? value)
    {
        return int.TryParse(value, CultureInfo.InvariantCulture, out int port) ? port : DefaultPort;
    }

    /// <summary>
    /// Provides the Resolve Async value or operation.
    /// </summary>
    public static async Task<(string Host, int Port)> ResolveAsync(string[] arguments, CancellationToken cancellationToken)
    {
        string host = arguments.ElementAtOrDefault(index: 0) ?? DefaultUpstreamHost;
        string resolvedHost = await ResolveHostAsync(host, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return (resolvedHost, ParsePort(arguments.ElementAtOrDefault(index: 1)));
    }

    private static async Task<string> ResolveHostAsync(string host, CancellationToken cancellationToken)
    {
        if (IPAddress.IsValid(host))
            return host;

        string? resolvedAddress = await TryResolveWithDnsAsync(host, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return resolvedAddress
            ?? await ResolveWithDnsOverSecureWebAsync(host, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private static async Task<string> ResolveWithDnsOverSecureWebAsync(string host, CancellationToken cancellationToken)
    {
        using HttpClient webClient = new();

        using HttpRequestMessage request = new(HttpMethod.Get, $"https://1.1.1.1/dns-query?name={Uri.EscapeDataString(host)}&type=A");

        request.Headers.Accept.ParseAdd(input: "application/dns-json");

        using HttpResponseMessage response = await webClient
            .SendAsync(request, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);


        Stream content = await response.EnsureSuccessStatusCode()
            .Content.ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        await using (content.ConfigureAwait(continueOnCapturedContext: false))
        {
            NameResolutionResponse document = await JsonSerializer.DeserializeAsync<NameResolutionResponse>(content, cancellationToken: cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false)
                ?? throw new InvalidDataException(message: "DNS-over-HTTPS returned a null response.");

            foreach (NameResolutionAnswer answer in document.Answers)
            {
                if (IPAddress.TryParse(answer.Address, out IPAddress? parsed) && parsed.AddressFamily is AddressFamily.InterNetwork)
                    return answer.Address;
            }

            throw new InvalidDataException($"DNS-over-HTTPS returned no IPv4 address for {host}.");
        }
    }

    private static async Task<string?> TryResolveWithDnsAsync(string host, CancellationToken cancellationToken)
    {
        try
        {
            LookupClient lookupClient = new(new LookupClientOptions(IPAddress.Parse(ipString: "1.1.1.1")) { Timeout = TimeSpan.FromSeconds(seconds: 2), Retries = 0, });

            IDnsQueryResponse response = await lookupClient
                .QueryAsync(host, QueryType.A, cancellationToken: cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            return response.Answers.ARecords().FirstOrDefault()?.Address.ToString();
        }
        catch (Exception exception)
            when (exception is DnsResponseException or OperationCanceledException or SocketException or TimeoutException)
        {
            return null;
        }
    }
}
