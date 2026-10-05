using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>Describes the player level required by a crop-growth event.</summary>
public sealed record CropGrowthEventRequirements
{
    /// <summary>Gets the inclusive minimum player level.</summary>
    [JsonPropertyName("minLevel")]
    public int MinimumLevel { get; init; }
}
