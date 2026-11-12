using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Preserves a queued production record and its product identity.</summary>
public sealed record PendingProductionSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the extra products awarded by the neighborhood perk when production started.</summary>
    /// <value>How many additional copies of the product the perk added, on top of the one ordered; 0 by default.</value>
    public int BonusProductAmount { get; init; }
    /// <summary>Gets or sets whether this production is an additional perk output.</summary>
    /// <value><see langword="true"/> if this entry is an extra output of the neighborhood perk; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag defaults to <see langword="false"/>.</remarks>
    public bool CreatedFromBonusProduct { get; init; }
    /// <summary>Gets or sets the product's native data id.</summary>
    /// <value>The global id of the product's table row, saved under the <c>ID</c> key.</value>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }

    /// <summary>Gets or sets the diamonds spent to complete this production instantly.</summary>
    /// <value>The diamond count saved under <c>diamondsSpentToInstantComplete</c>, normally 0 for a production still in the queue; <see langword="null"/> when the key is absent.</value>
    [JsonPropertyName("diamondsSpentToInstantComplete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DiamondsSpent { get; init; }

    /// <summary>Gets or sets the helper that started this production, when present.</summary>
    /// <value>0 for the caretaker, 1 for the miller, 2 for Angus; <see langword="null"/> when the player started the production.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? HelperType { get; init; }

    /// <summary>Gets or sets the original production duration, when saved.</summary>
    /// <value>The duration the game recorded before any reduction; <see langword="null"/> when the key is absent, as in most saved queues, where <see cref="Timer"/> carries the duration instead.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ProductionTime { get; init; }

    /// <summary>Gets or sets the seconds a movie-ticket speed up took off this production.</summary>
    /// <value>A positive number of seconds once the speed up was used; <see langword="null"/> (read as 0) when none has been applied.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ProductionTimeCut { get; init; }

    /// <summary>Gets or sets the reduced production duration, when saved.</summary>
    /// <value>The duration after reductions were applied, as the game recorded it; <see langword="null"/> when the key is absent.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ReducedProductionTime { get; init; }

    /// <summary>Gets or sets the movie-ticket speed up, in seconds, the game has already shown the player for this production.</summary>
    /// <value>A number of seconds; <see langword="null"/> when nothing was shown.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TimeCutShownFromVideoAd { get; init; }

    /// <summary>Gets or sets the saved timer for this queued production.</summary>
    /// <value>The countdown, with ticks at 15 per second; <see langword="null"/> loads as a zero timer.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? Timer { get; init; }
}
