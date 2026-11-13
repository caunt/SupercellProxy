using System.Collections.Immutable;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Retains a day's ordinary Valley shop choices and their promotion boxes.</summary>
public sealed record MapGamePromotionRewardGroupSnapshot
{
    /// <summary>Gets or sets the native day key.</summary>
    /// <value>The day the group's choices were offered on.</value>
    public int Day { get; init; }

    /// <summary>Gets or sets the promotion boxes attached to the reward choices.</summary>
    /// <value>Boxes tied to choices by their index in <see cref="Rewards"/>; a choice without an entry has no box. Empty by default.</value>
    public ImmutableArray<MapGamePromotionRewardSnapshot> PopBoxes { get; init; } = [];

    /// <summary>Gets or sets the shop reward data identifiers, in choice order.</summary>
    /// <value>Global ids of the offered rows. Empty by default.</value>
    public ImmutableArray<int> Rewards { get; init; } = [];
}
