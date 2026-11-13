using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>Configures the crops affected by a growth-duration or harvest-yield event.</summary>
public sealed record CropGrowthEventDefinition
{
    /// <summary>Gets or sets the affected crop data names.</summary>
    /// <value>Names of rows in the fields table, read from <c>crops</c>; with a positive <see cref="PerPlayerCropLimit"/> only a per-player selection of them is affected. Empty by default.</value>
    [JsonPropertyName("crops")]
    public ImmutableArray<string> Crops { get; init; } = [];

    /// <summary>Gets or sets the retained per-player crop limit.</summary>
    /// <value>How many of <see cref="Crops"/> each player gets the bonus for, read from <c>perPlayerCropsLimit</c>; 0 or less applies the bonus to every listed crop.</value>
    [JsonPropertyName("perPlayerCropsLimit")]
    public int PerPlayerCropLimit { get; init; }

    /// <summary>Gets or sets the event's player eligibility requirements.</summary>
    /// <value>The requirements read from <c>requirements</c>, or <see langword="null"/> when the event sets none.</value>
    [JsonPropertyName("requirements")]
    public CropGrowthEventRequirements? Requirements { get; init; }

    /// <summary>Gets or sets the percentage of the catalog growth duration used when planting.</summary>
    /// <value>A percentage read from <c>cropPercentTime</c>, such as 50 for half the growth time; 100 by default, which leaves the duration unchanged.</value>
    [JsonPropertyName("cropPercentTime")]
    public int TimePercentage { get; init; } = 100;

    /// <summary>Gets or sets the percentage of the catalog harvest quantity credited by a yield event.</summary>
    /// <value>A percentage read from <c>cropYield</c>, such as 200 for double yield; 100 by default, which leaves the harvest unchanged.</value>
    [JsonPropertyName("cropYield")]
    public int YieldPercentage { get; init; } = 100;
}
