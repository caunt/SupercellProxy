using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.AppStore;

internal readonly record struct AppStoreSearchResponse([property: JsonPropertyName("results")] AppStoreSearchResult[] Results);
