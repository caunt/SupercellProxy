using System.Text.Json.Serialization;

namespace SupercellProxy.Keys;

internal readonly record struct AppStoreSearchResponse(
    [property: JsonPropertyName("results")] AppStoreSearchResult[] Results
);
