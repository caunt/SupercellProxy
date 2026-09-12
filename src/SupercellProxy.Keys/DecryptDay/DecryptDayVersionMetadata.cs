using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>Represents the typed DecryptDayVersionMetadata response contract.</summary>
internal sealed record DecryptDayVersionMetadata
{
    /// <summary>Gets the Name value.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

}
