using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Saved state of a hireable helper, such as the caretaker, the miller or Angus: offers, hire time and orders.</summary>
public sealed record HelperHouseStateSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the pending post-hire state, when retained.</summary>
    /// <value>The state the helper moves to when the current hire ends; <see langword="null"/> when not saved.</value>
    [JsonPropertyName("BoosterPendingState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? BoosterPendingState { get; init; }

    /// <summary>Gets or sets the free-offer cooldown timer, when retained.</summary>
    /// <value>The countdown used while <see cref="State"/> is 1; <see langword="null"/> loads as an elapsed timer.</value>
    [JsonPropertyName("FreeTimer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? FreeTimer { get; init; }

    /// <summary>Gets or sets Angus's automatic tackle targets in native goods order.</summary>
    /// <value>One flag per net and then per bait, in table order, where any non-zero entry marks that tackle as one Angus keeps stocked automatically; <see langword="null"/> means none is.</value>
    [JsonPropertyName("LureAutoTargets")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? LureAutoTargets { get; init; }

    /// <summary>
    /// Gets or sets the caretaker's feeding mode, when retained: 0 also feeds every hungry animal, 1 also keeps spare feed, and 2 works for the
    /// orders only.
    /// </summary>
    /// <value>The mode code chosen by the player; <see langword="null"/> when the save has none, which is read as 1, the game's default.</value>
    [JsonPropertyName("Mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Mode { get; init; }

    /// <summary>Gets or sets whether the player turned down the current offer.</summary>
    /// <value><see langword="true"/> if the player dismissed the offer now shown; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("OfferDeclined")]
    public bool OfferDeclined { get; init; }

    /// <summary>Gets or sets the requested quantities in the helper's native goods order, when retained.</summary>
    /// <value>One amount per good the helper handles, in that helper's table order (for Angus, nets then baits); <see langword="null"/> is read as no orders.</value>
    [JsonPropertyName("Orders")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? Orders { get; init; }

    /// <summary>
    /// Gets or sets where the helper is: 1 waiting for the free offer, 2 hired and working, 3 showing a paid offer for a while, 4 offering a
    /// free hire.
    /// </summary>
    /// <value>The helper's state code.</value>
    [JsonPropertyName("State")]
    public int State { get; init; }

    /// <summary>Gets or sets the remaining hire or offer duration, when retained.</summary>
    /// <value>The countdown of the hire while <see cref="State"/> is 2 and of the paid offer while it is 3; <see langword="null"/> loads as an elapsed timer.</value>
    [JsonPropertyName("Timer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? Timer { get; init; }
}
