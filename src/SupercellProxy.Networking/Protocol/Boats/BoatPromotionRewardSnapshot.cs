namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Retains a crate's promotion reward and collection marker.</summary>
public sealed record BoatPromotionRewardSnapshot
{
    /// <summary>Gets the optional resource reward.</summary>
    public BoatResourceRewardSnapshot? Reward { get; init; }
    /// <summary>Gets whether the promotion reward was collected.</summary>
    public bool RewardCollected { get; init; }
}
