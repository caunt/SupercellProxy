using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

using DnsClient;

namespace SupercellProxy.Networking.Transport;

/// <summary>
/// Default game server address and port, and command-line address parsing that resolves host names to an IPv4 address without
/// relying on the machine's own DNS settings.
/// </summary>
public static class ConnectionAddressResolver
{
    /// <summary>
    /// Address the proxy listens on unless configured otherwise: every IPv4 interface.
    /// </summary>
    public const string DefaultListenHost = "0.0.0.0";

    /// <summary>
    /// TCP port the game uses, for both the real server and the proxy's listener.
    /// </summary>
    public const int DefaultPort = 9339;

    /// <summary>
    /// Host name of the live Hay Day game server.
    /// </summary>
    public const string DefaultUpstreamHost = "game.haydaygame.com";

    /// <summary>
    /// Reads a port number from text, falling back to <see cref="DefaultPort"/> when the text is missing or not a number.
    /// </summary>
    /// <param name="value">Port as typed by the user, or null.</param>
    public static int ParsePort(string? value)
    {
        return int.TryParse(value, CultureInfo.InvariantCulture, out int port) ? port : DefaultPort;
    }

    /// <summary>
    /// Turns command-line arguments (host, then port) into an IPv4 address and port. Host names are looked up at 1.1.1.1 directly,
    /// then through Cloudflare's DNS-over-HTTPS when plain DNS fails.
    /// </summary>
    /// <param name="arguments">Optional host as the first argument and optional port as the second; missing ones use the defaults.</param>
    /// <param name="cancellationToken">Stops the lookups.</param>
    /// <exception cref="InvalidDataException">No lookup produced an IPv4 address.</exception>
    public static async Task<ConnectionAddress> ResolveAsync(string[] arguments, CancellationToken cancellationToken)
    {
        string host = arguments.ElementAtOrDefault(index: 0) ?? DefaultUpstreamHost;
        string resolvedHost = await ResolveHostAsync(host, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return new ConnectionAddress(resolvedHost, ParsePort(arguments.ElementAtOrDefault(index: 1)));
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
