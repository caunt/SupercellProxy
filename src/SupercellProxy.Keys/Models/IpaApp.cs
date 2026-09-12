namespace SupercellProxy.Keys.Models;

internal sealed record IpaApp(
    [property: System.Text.Json.Serialization.JsonPropertyName("BundleId")] string BundleIdentifier,
    IReadOnlyList<AppVersion> Versions
);
