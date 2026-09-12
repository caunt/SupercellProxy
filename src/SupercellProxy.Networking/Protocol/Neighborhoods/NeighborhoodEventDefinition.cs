using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Represents the decoded NeighborhoodEventDefinition JSON contract.</summary>
public sealed record NeighborhoodEventDefinition
{
    /// <summary>Gets the TaskSet value.</summary>
    [JsonPropertyName("taskSet")]
    public string[] TaskSet { get; init; } = [];

    /// <summary>Gets the TaskSetsPerWeek value.</summary>
    [JsonPropertyName("taskSetsPerWeek")]
    public int TaskSetsPerWeek { get; init; }

    /// <summary>Gets the TaskSlotCount value.</summary>
    [JsonPropertyName("taskSlotCount")]
    public int TaskSlotCount { get; init; }

}
