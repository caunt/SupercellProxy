using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Events;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Configures tree and bush growth during a Chronos event.</summary>
public sealed record FruitGrowthEventDefinition
{
    /// <summary>Gets the affected source names; an empty list covers every fruit source.</summary>
    [JsonPropertyName("fruitTrees")]
    public string[] FruitTrees { get; init; } = [];

    /// <summary>Gets the participation level requirement.</summary>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }

    /// <summary>Gets the percentage of the normal growth duration.</summary>
    [JsonPropertyName("fruitTreePercentTime")]
    public int TimePercentage { get; init; } = 100;
}
