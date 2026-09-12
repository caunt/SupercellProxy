namespace SupercellProxy.Keys.Models;

internal sealed record DecryptDayAppDetail(
    [property: System.Text.Json.Serialization.JsonPropertyName("Id")] string Identifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("BundleId")] string BundleIdentifier,
    IReadOnlyList<string> Versions
);
