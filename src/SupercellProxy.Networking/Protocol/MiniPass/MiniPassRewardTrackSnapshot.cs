using System.Collections.Immutable;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved rewards of one Mini Pass track.</summary>
public sealed record MiniPassRewardTrackSnapshot
{
    /// <summary>Gets or sets the claimed-reward bit set.</summary>
    /// <value>One bit per reward index, set once that reward is claimed; <see langword="null"/> means nothing has been claimed yet.</value>
    public MiniPassClaimedRewardsSnapshot? Claimed { get; init; }

    /// <summary>Gets or sets the reward amounts by index.</summary>
    /// <value>How many of the item in <see cref="Ids"/> at the same index the reward grants.</value>
    public ImmutableArray<int> Counts { get; init; } = [];

    /// <summary>Gets or sets the reward data rows by index.</summary>
    /// <value>Global ids of the rewarded items, such as coins, goods or gacha currency, one per reward index on the track.</value>
    public ImmutableArray<int> Ids { get; init; } = [];
}
