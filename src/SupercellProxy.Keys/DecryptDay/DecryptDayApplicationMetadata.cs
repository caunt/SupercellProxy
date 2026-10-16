using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>The app a decrypt.day page describes.</summary>
internal sealed record DecryptDayApplicationMetadata
{
    /// <summary>The app's iOS bundle identifier, such as com.supercell.hayday.</summary>
    [JsonPropertyName("bundle_id")]
    public string? BundleId { get; init; }

    /// <summary>decrypt.day's own identifier for the app, used to ask for its files.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}
