using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved claimed-reward bit set of one Mini Pass track.</summary>
public sealed record MiniPassClaimedRewardsSnapshot
{
    /// <summary>Gets or sets the bit words, 32 rewards per word.</summary>
    /// <value>
    /// Saved as <c>bits</c>; reward index <c>i</c> is claimed when bit <c>i % 32</c> of word <c>i / 32</c> is set. Empty by default, and a
    /// missing word means none of its rewards are claimed.
    /// </value>
    [JsonPropertyName("bits")]
    public ImmutableArray<int> Bits { get; init; } = [];
}
