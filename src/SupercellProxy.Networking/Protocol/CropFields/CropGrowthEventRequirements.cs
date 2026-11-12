using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>Describes the player level required by a crop-growth event.</summary>
public sealed record CropGrowthEventRequirements
{
    /// <summary>Gets or sets the inclusive minimum player level.</summary>
    /// <value>The lowest experience level that gets the event's effect, read from <c>minLevel</c>; 0 by default, which admits every player.</value>
    [JsonPropertyName("minLevel")]
    public int MinimumLevel { get; init; }
}
