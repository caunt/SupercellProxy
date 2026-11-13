using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>A set of Farm Pass level rewards collected together, with its collected flag.</summary>
public sealed record FarmPassRewardGroup
{
    /// <summary>Gets or sets whether the player already collected the set.</summary>
    /// <value><see langword="true"/> if the level reward claim has granted this set; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("rewardCollected")]
    public bool Collected { get; set; }

    /// <summary>Gets or sets what the set gives, each resolved to its table entry and amount.</summary>
    /// <value>The rewards granted together on claim; empty by default.</value>
    [JsonPropertyName("rewards")]
    public ImmutableArray<FarmPassRewardItem> Rewards { get; init; } = [];
}
