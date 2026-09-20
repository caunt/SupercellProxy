using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Represents decoded ProductionListSnapshot state.</summary>
public sealed record ProductionListSnapshot : ExtensibleDocument
{
    /// <summary>Gets the FinishedProductions value.</summary>
    [JsonPropertyName("FinishedProductions")]
    public FinishedProductionSnapshot[] FinishedProductions { get; init; } = [];

    /// <summary>Gets retained queued productions, when present.</summary>
    [JsonPropertyName("Productions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PendingProductionSnapshot[]? Productions { get; init; }
}
