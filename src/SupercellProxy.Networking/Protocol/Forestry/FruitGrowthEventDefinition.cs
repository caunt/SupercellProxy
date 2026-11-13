using System.Collections.Immutable;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Events;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Configures tree and bush growth during a Chronos event.</summary>
public sealed record FruitGrowthEventDefinition
{
    /// <summary>Gets or sets the affected source names; an empty list covers every fruit source.</summary>
    /// <value>Row names of the fruit trees table, read from <c>fruitTrees</c>.</value>
    [JsonPropertyName("fruitTrees")]
    public ImmutableArray<string> FruitTrees { get; init; } = [];

    /// <summary>Gets or sets the participation level requirement.</summary>
    /// <value>The event's minimum level, read from <c>requirements</c>; <see langword="null"/> when the event sets no level.</value>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }

    /// <summary>Gets or sets the percentage of the normal growth duration.</summary>
    /// <value>A percentage applied to each phase's <c>TimeMin</c>; defaults to 100, which leaves growth unchanged, and 50 halves it.</value>
    [JsonPropertyName("fruitTreePercentTime")]
    public int TimePercentage { get; init; } = 100;
}
