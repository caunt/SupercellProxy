using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Saved state of a hireable helper, such as the caretaker, the miller or Angus: offers, hire time and orders.</summary>
public sealed record HelperHouseStateSnapshot : ExtensibleDocument
{
    /// <summary>Gets the pending post-hire state, when retained.</summary>
    [JsonPropertyName("BoosterPendingState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? BoosterPendingState { get; init; }

    /// <summary>Gets the free-offer cooldown timer, when retained.</summary>
    [JsonPropertyName("FreeTimer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? FreeTimer { get; init; }

    /// <summary>Gets Angus's automatic tackle targets in native goods order.</summary>
    [JsonPropertyName("LureAutoTargets")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? LureAutoTargets { get; init; }

    /// <summary>Whether the player turned down the current offer; a new offer clears it.</summary>
    [JsonPropertyName("OfferDeclined")]
    public bool OfferDeclined { get; init; }

    /// <summary>Gets the requested quantities in the helper's native goods order, when retained.</summary>
    [JsonPropertyName("Orders")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? Orders { get; init; }

    /// <summary>
    /// Where the helper is: 1 waiting for the free offer, 2 hired and working, 3 showing a paid offer for a while, 4 offering a
    /// free hire.
    /// </summary>
    [JsonPropertyName("State")]
    public int State { get; init; }

    /// <summary>Gets the remaining hire or offer duration, when retained.</summary>
    [JsonPropertyName("Timer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? Timer { get; init; }
}
