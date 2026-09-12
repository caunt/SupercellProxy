using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>Represents the typed DecryptDayApplicationMetadata response contract.</summary>
internal sealed record DecryptDayApplicationMetadata
{

    /// <summary>Gets the BundleIdentifier value.</summary>
    [JsonPropertyName("bundle_id")]
    public string? BundleIdentifier { get; init; }
    /// <summary>Gets the Identifier value.</summary>
    [JsonPropertyName("id")]
    public string? Identifier { get; init; }

}
