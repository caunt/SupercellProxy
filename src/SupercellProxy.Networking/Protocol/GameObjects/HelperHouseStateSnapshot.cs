using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Represents decoded HelperHouseStateSnapshot state.</summary>
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

    /// <summary>Gets the OfferDeclined value.</summary>
    [JsonPropertyName("OfferDeclined")]
    public bool OfferDeclined { get; init; }

    /// <summary>Gets the requested quantities in the helper's native goods order, when retained.</summary>
    [JsonPropertyName("Orders")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? Orders { get; init; }

    /// <summary>Gets the State value.</summary>
    [JsonPropertyName("State")]
    public int State { get; init; }

    /// <summary>Gets the remaining hire or offer duration, when retained.</summary>
    [JsonPropertyName("Timer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? Timer { get; init; }
}
