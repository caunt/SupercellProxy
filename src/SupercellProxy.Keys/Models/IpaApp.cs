namespace SupercellProxy.Keys.Models;

internal sealed record IpaApp([property: System.Text.Json.Serialization.JsonPropertyName("BundleId")] string BundleId, IReadOnlyList<AppVersion> Versions);
