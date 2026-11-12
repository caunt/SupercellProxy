using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>A finished product waiting in a production building to be collected.</summary>
public sealed record FinishedProductionSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the saved neighborhood perk bonus count.</summary>
    /// <value>The number of extra copies the neighborhood perk added to the production this product came from; 0 when the perk gave none.</value>
    public int BonusProductAmount { get; init; }

    /// <summary>Gets or sets whether this finished product was created by a neighborhood perk.</summary>
    /// <value><see langword="true"/> if this entry is one of the perk's extra copies rather than the ordered product; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag defaults to <see langword="false"/>.</remarks>
    public bool CreatedFromBonusProduct { get; init; }

    /// <summary>Gets or sets the global id of the product's table entry (saved as <c>ID</c>).</summary>
    /// <value>The row of the finished good.</value>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }

    /// <summary>Gets or sets the diamonds spent to finish the product instantly; <see langword="null"/> when none were spent.</summary>
    /// <value>The diamond count saved as <c>diamondsSpentToInstantComplete</c> when the player skipped the wait; the key is omitted otherwise.</value>
    [JsonPropertyName("diamondsSpentToInstantComplete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DiamondsSpent { get; init; }

    /// <summary>Gets or sets the helper that ordered the product: 0 the caretaker, 1 the miller, 2 Angus; <see langword="null"/> when the player did.</summary>
    /// <value>The helper type code saved with the product; the key is omitted for the player's own products.</value>
    [JsonPropertyName("HelperType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? HelperType { get; init; }

    /// <summary>Gets or sets how long the product took to make; <see langword="null"/> when not saved.</summary>
    /// <value>The production duration as the game recorded it before any reduction; the key is omitted when the game did not record one.</value>
    [JsonPropertyName("ProductionTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ProductionTime { get; init; }

    /// <summary>Gets or sets how much of the production time boosters took off; <see langword="null"/> when not saved.</summary>
    /// <value>The booster reduction the game recorded for this product; the key is omitted when nothing was recorded.</value>
    [JsonPropertyName("ReducedProductionTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ReducedProductionTime { get; init; }
}
