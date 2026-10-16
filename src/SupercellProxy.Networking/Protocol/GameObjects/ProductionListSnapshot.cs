using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Saved production queue of a building: finished products waiting to be collected and products still being made.</summary>
public sealed record ProductionListSnapshot : ExtensibleDocument
{
    /// <summary>Finished products waiting to be collected, oldest first.</summary>
    [JsonPropertyName("FinishedProductions")]
    public FinishedProductionSnapshot[] FinishedProductions { get; init; } = [];

    /// <summary>Gets retained queued productions, when present.</summary>
    [JsonPropertyName("Productions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PendingProductionSnapshot[]? Productions { get; init; }
}
