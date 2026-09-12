using System.Text.Json;

namespace SupercellProxy.Keys.AppStore;

internal sealed class AppStoreClient(HttpClient client)
{
    private const string Country = "us";
    private readonly HttpClient _client = client;

    public async Task<AppStoreSearchResponse> SearchAsync(string query, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query);

        string address =
            $"https://itunes.apple.com/search?entity=software,iPadSoftware&country={Country}&limit=20"
            + $"&term={Uri.EscapeDataString(query)}";

        using HttpResponseMessage response = await _client
            .SendWithRetryAsync(() => new HttpRequestMessage(HttpMethod.Get, address), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);


        Stream content = await response.EnsureSuccessStatusCode()
            .Content.ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        await using (content.ConfigureAwait(continueOnCapturedContext: false))
        {
            return await JsonSerializer
                    .DeserializeAsync<AppStoreSearchResponse?>(content, cancellationToken: cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false)
                ?? throw new InvalidDataException(message: "The App Store returned an empty response.");
        }
    }
}
