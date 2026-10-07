namespace SupercellProxy.Networking.Protocol.Events.Derby.Rewards;

/// <summary>Preserves a saved derby reward pool or a generated podium reward list.</summary>
public sealed record DerbyRewardPoolSnapshot
{
    /// <summary>Gets the reward data identifiers in native order.</summary>
    public int[] Rewards { get; init; } = [];
    /// <summary>Gets the selection weight corresponding to each reward, when this is a pool.</summary>
    public int[]? Probability { get; init; }
    /// <summary>Gets the quantity corresponding to each reward.</summary>
    public int[] Amounts { get; init; } = [];
}
