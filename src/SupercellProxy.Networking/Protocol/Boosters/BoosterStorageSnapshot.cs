using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Represents the decoded <c>BoosterStorage</c> avatar-data document.</summary>
public sealed record BoosterStorageSnapshot
{
    /// <summary>Gets or sets the retained booster storage slots.</summary>
    /// <value>The stored boosters saved as <c>BoosterSlotsV3</c>, one entry per booster in saved order; empty by default.</value>
    [JsonPropertyName("BoosterSlotsV3")]
    public ImmutableArray<BoosterSlotSnapshot> BoosterSlots { get; init; } = [];
}
