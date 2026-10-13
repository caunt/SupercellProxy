namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved rewards of one Mini Pass track.</summary>
public sealed record MiniPassRewardTrackSnapshot
{
    /// <summary>Gets the claimed-reward bit set.</summary>
    public MiniPassClaimedRewardsSnapshot? Claimed { get; init; }

    /// <summary>Gets the reward amounts by index.</summary>
    public int[] Counts { get; init; } = [];

    /// <summary>Gets the reward data rows by index.</summary>
    public int[] Ids { get; init; } = [];
}
