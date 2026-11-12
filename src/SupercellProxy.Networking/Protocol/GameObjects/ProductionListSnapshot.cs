using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Saved production queue of a building: finished products waiting to be collected and products still being made.</summary>
public sealed record ProductionListSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the finished products waiting to be collected, oldest first.</summary>
    /// <value>One entry per finished product; empty by default and when nothing is ready.</value>
    [JsonPropertyName("FinishedProductions")]
    public FinishedProductionSnapshot[] FinishedProductions { get; init; } = [];

    /// <summary>Gets or sets the queued productions still being made, when present.</summary>
    /// <value>The queue in order; <see langword="null"/> when the key is absent, which is read as an empty queue.</value>
    [JsonPropertyName("Productions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PendingProductionSnapshot[]? Productions { get; init; }
}
