using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.AppStore;

internal readonly record struct AppStoreSearchResult(
    [property: JsonPropertyName("trackId")] long TrackIdentifier,
    [property: JsonPropertyName("trackCensoredName")] string Name,
    [property: JsonPropertyName("bundleId")] string BundleIdentifier,
    [property: JsonPropertyName("sellerName")] string? SellerName
);
