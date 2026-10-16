using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>A finished product waiting in a production building to be collected.</summary>
public sealed record FinishedProductionSnapshot : ExtensibleDocument
{
    /// <summary>Gets the saved neighborhood perk bonus count.</summary>
    public int BonusProductAmount { get; init; }

    /// <summary>Gets whether this finished product was created by a neighborhood perk.</summary>
    public bool CreatedFromBonusProduct { get; init; }

    /// <summary>Global id of the product's table entry (saved as <c>ID</c>).</summary>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }

    /// <summary>Diamonds spent to finish the product instantly; null when none were spent.</summary>
    [JsonPropertyName("diamondsSpentToInstantComplete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DiamondsSpent { get; init; }

    /// <summary>Helper that ordered the product: 0 the caretaker, 1 the miller, 2 Angus; null when the player did.</summary>
    [JsonPropertyName("HelperType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? HelperType { get; init; }

    /// <summary>How long the product took to make; null when not saved.</summary>
    [JsonPropertyName("ProductionTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ProductionTime { get; init; }

    /// <summary>How much of the production time boosters took off; null when not saved.</summary>
    [JsonPropertyName("ReducedProductionTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ReducedProductionTime { get; init; }
}
