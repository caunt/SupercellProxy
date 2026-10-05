using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>Configures the crops and planting duration affected by a crop-growth event.</summary>
public sealed record CropGrowthEventDefinition
{
    /// <summary>Gets the affected crop data names.</summary>
    [JsonPropertyName("crops")]
    public string[] Crops { get; init; } = [];

    /// <summary>Gets the retained per-player crop limit.</summary>
    [JsonPropertyName("perPlayerCropsLimit")]
    public int PerPlayerCropLimit { get; init; }

    /// <summary>Gets the event's player eligibility requirements.</summary>
    [JsonPropertyName("requirements")]
    public CropGrowthEventRequirements? Requirements { get; init; }

    /// <summary>Gets the percentage of the catalog growth duration used when planting.</summary>
    [JsonPropertyName("cropPercentTime")]
    public int TimePercentage { get; init; } = 100;
}
