using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>Represents the typed DecryptDayApplicationMetadata response contract.</summary>
internal sealed record DecryptDayApplicationMetadata
{
    /// <summary>Gets the BundleId value.</summary>
    [JsonPropertyName("bundle_id")]
    public string? BundleId { get; init; }
    /// <summary>Gets the Id value.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}
