using System.Text.Json;

namespace SupercellProxy.Keys;

internal sealed class AppStoreClient(HttpClient client)
{
    private const string Country = "us";

    public async Task<AppStoreSearchResponse> SearchAsync(
        string query,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query);

        var url = $"https://itunes.apple.com/search?entity=software,iPadSoftware&country={Country}&limit=20" +
                  $"&term={Uri.EscapeDataString(query)}";

        using var response = await client.SendWithRetryAsync(
            () => new HttpRequestMessage(HttpMethod.Get, url),
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await JsonSerializer.DeserializeAsync<AppStoreSearchResponse>(
                   await response.Content.ReadAsStreamAsync(cancellationToken),
                   cancellationToken: cancellationToken)
               ?? throw new InvalidDataException("The App Store returned an empty response.");
    }
}
