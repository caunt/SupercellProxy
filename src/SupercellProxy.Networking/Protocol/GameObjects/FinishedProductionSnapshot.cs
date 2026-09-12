using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Represents decoded FinishedProductionSnapshot state.</summary>
public sealed record FinishedProductionSnapshot : ExtensibleDocument
{
    /// <summary>Gets the DataGlobalIdentifier value.</summary>
    [JsonPropertyName("ID")]
    public int DataGlobalIdentifier { get; init; }

    /// <summary>Gets the DiamondsSpent value.</summary>
    [JsonPropertyName("diamondsSpentToInstantComplete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DiamondsSpent { get; init; }

    /// <summary>Gets the HelperType value.</summary>
    [JsonPropertyName("HelperType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? HelperType { get; init; }

    /// <summary>Gets the ProductionTime value.</summary>
    [JsonPropertyName("ProductionTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ProductionTime { get; init; }

    /// <summary>Gets the ReducedProductionTime value.</summary>
    [JsonPropertyName("ReducedProductionTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ReducedProductionTime { get; init; }

}
