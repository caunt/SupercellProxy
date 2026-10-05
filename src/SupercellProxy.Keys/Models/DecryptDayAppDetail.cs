namespace SupercellProxy.Keys.Models;

internal sealed record DecryptDayAppDetail(
    [property: System.Text.Json.Serialization.JsonPropertyName("Id")] string Id,
    [property: System.Text.Json.Serialization.JsonPropertyName("BundleId")] string BundleId,
    IReadOnlyList<string> Versions
);
