using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved claimed-reward bit set of one Mini Pass track.</summary>
public sealed record MiniPassClaimedRewardsSnapshot
{
    /// <summary>Gets the bit words, 32 rewards per word.</summary>
    [JsonPropertyName("bits")]
    public int[] Bits { get; init; } = [];
}
