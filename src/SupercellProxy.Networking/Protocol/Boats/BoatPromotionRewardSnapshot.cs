namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Retains a crate's promotion reward and collection marker.</summary>
public sealed record BoatPromotionRewardSnapshot
{
    /// <summary>Gets or sets the optional resource reward.</summary>
    /// <value>The resource id and quantity the promotion box grants, or <see langword="null"/> when the save carries no reward object.</value>
    public BoatResourceRewardSnapshot? Reward { get; init; }
    /// <summary>Gets or sets a value indicating whether the promotion reward was collected.</summary>
    /// <value><see langword="true"/> if the player already collected the box's reward; otherwise, <see langword="false"/>.</value>
    /// <remarks>A box still waiting to be collected keeps the default <see langword="false"/>.</remarks>
    public bool RewardCollected { get; init; }
}
