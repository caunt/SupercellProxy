using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Represents the decoded <c>BoosterStorage</c> avatar-data document.</summary>
public sealed record BoosterStorageSnapshot
{
    /// <summary>Gets the retained booster storage slots.</summary>
    [JsonPropertyName("BoosterSlotsV3")]
    public BoosterSlotSnapshot[] BoosterSlots { get; init; } = [];
}
