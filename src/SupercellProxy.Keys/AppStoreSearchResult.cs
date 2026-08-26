using System.Text.Json.Serialization;

namespace SupercellProxy.Keys;

internal sealed record AppStoreSearchResult(
    [property: JsonPropertyName("trackId")] long TrackId,
    [property: JsonPropertyName("trackCensoredName")] string Name,
    [property: JsonPropertyName("bundleId")] string BundleId,
    [property: JsonPropertyName("sellerName")] string? SellerName
);
