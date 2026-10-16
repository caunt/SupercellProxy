using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>One app release decrypt.day knows about.</summary>
internal sealed record DecryptDayVersionMetadata
{
    /// <summary>Release number as text, such as 1.73.81.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
