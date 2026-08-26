using System.Text.Json;

namespace SupercellProxy.Keys;

internal sealed class AppStoreClient(HttpClient client)
{
    private const string Country = "us";
    private readonly HttpClient client = client;

    public async Task<AppStoreSearchResponse> SearchAsync(
        string query,
        CancellationToken cancellationToken
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query);

        var url =
            $"https://itunes.apple.com/search?entity=software,iPadSoftware&country={Country}&limit=20"
            + $"&term={Uri.EscapeDataString(query)}";

        using var response = await client
            .SendWithRetryAsync(
                () => new HttpRequestMessage(HttpMethod.Get, url),
                cancellationToken
            )
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var content = await response
            .Content.ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);
        await using (content.ConfigureAwait(false))
        {
            return await JsonSerializer
                    .DeserializeAsync<AppStoreSearchResponse>(
                        content,
                        cancellationToken: cancellationToken
                    )
                    .ConfigureAwait(false)
                ?? throw new InvalidDataException("The App Store returned an empty response.");
        }
    }
}
