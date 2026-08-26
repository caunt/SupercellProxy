using System.Text.Json.Serialization;

namespace SupercellProxy.Keys;

internal sealed record AppStoreSearchResponse(
    [property: JsonPropertyName("results")] AppStoreSearchResult[] Results
);
