using System.Text.Json.Serialization;
namespace SupercellProxy.Keys.Models;

internal sealed record DecryptDayAppDetail(
    [property: JsonPropertyName("Id")] string Id,
    [property: JsonPropertyName("BundleId")] string BundleId,
    IReadOnlyList<string> Versions
);
