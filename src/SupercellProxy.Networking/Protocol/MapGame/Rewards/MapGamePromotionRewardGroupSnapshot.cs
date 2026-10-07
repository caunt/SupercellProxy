namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Retains a day's ordinary Valley shop choices and their promotion boxes.</summary>
public sealed record MapGamePromotionRewardGroupSnapshot
{
    /// <summary>Gets the native day key.</summary>
    public int Day { get; init; }
    /// <summary>Gets the shop reward data identifiers, in choice order.</summary>
    public int[] Rewards { get; init; } = [];
    /// <summary>Gets promotion boxes attached to the reward choices.</summary>
    public MapGamePromotionRewardSnapshot[] PopBoxes { get; init; } = [];
}
