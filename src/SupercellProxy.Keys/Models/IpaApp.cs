using System.Text.Json.Serialization;
namespace SupercellProxy.Keys.Models;

internal sealed record IpaApp([property: JsonPropertyName("BundleId")] string BundleId, IReadOnlyList<AppVersion> Versions);
