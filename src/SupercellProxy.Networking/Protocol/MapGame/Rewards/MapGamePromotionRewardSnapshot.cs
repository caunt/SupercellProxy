using SupercellProxy.Networking.Protocol.Boats;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Associates one promotion box with an ordinary Valley shop choice.</summary>
public sealed record MapGamePromotionRewardSnapshot
{
    /// <summary>Gets or sets the index within the day's reward choices.</summary>
    /// <value>A zero-based position in the group's reward list.</value>
    public int PopBoxRewardIndex { get; init; }

    /// <summary>Gets or sets the attached promotion box.</summary>
    /// <value>The box's reward and whether it was collected; <see langword="null"/> when the entry carries no box.</value>
    public BoatPromotionRewardSnapshot? PopPromoBox { get; init; }
}
