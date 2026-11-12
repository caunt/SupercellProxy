namespace SupercellProxy.Networking.Protocol.Events.Derby.Rewards;

/// <summary>Preserves a saved derby reward pool or a generated podium reward list.</summary>
public sealed record DerbyRewardPoolSnapshot
{
    /// <summary>Gets or sets the reward data identifiers in native order.</summary>
    /// <value>The global data identifiers of the reward items; position <c>i</c> pairs with position <c>i</c> of <see cref="Amounts"/> and <see cref="Probability"/>. An empty array when none were saved.</value>
    public int[] Rewards { get; init; } = [];
    /// <summary>Gets or sets the selection weight corresponding to each reward, when this is a pool.</summary>
    /// <value>One relative weight per entry of <see cref="Rewards"/>, or <see langword="null"/> when the snapshot is a generated podium reward list rather than a pool.</value>
    public int[]? Probability { get; init; }
    /// <summary>Gets or sets the quantity corresponding to each reward.</summary>
    /// <value>How many of each reward item is granted, one count per entry of <see cref="Rewards"/>; an empty array when none were saved.</value>
    public int[] Amounts { get; init; } = [];
}
