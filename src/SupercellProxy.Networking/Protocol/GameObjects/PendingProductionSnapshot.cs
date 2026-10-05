using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Preserves a queued production record and its product identity.</summary>
public sealed record PendingProductionSnapshot : ExtensibleDocument
{
    /// <summary>Gets the product's native data id.</summary>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }

    /// <summary>Gets the diamonds spent to complete this production instantly.</summary>
    [JsonPropertyName("diamondsSpentToInstantComplete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DiamondsSpent { get; init; }

    /// <summary>Gets the helper that started this production, when present.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? HelperType { get; init; }

    /// <summary>Gets the original production duration, when saved.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ProductionTime { get; init; }

    /// <summary>Gets the reduced production duration, when saved.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ReducedProductionTime { get; init; }

    /// <summary>Gets the saved timer for this queued production.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? Timer { get; init; }
}
