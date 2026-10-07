using SupercellProxy.Networking.Protocol.Boats;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Associates one promotion box with an ordinary Valley shop choice.</summary>
public sealed record MapGamePromotionRewardSnapshot
{
    /// <summary>Gets the index within the day's reward choices.</summary>
    public int PopBoxRewardIndex { get; init; }
    /// <summary>Gets the attached promotion box.</summary>
    public BoatPromotionRewardSnapshot? PopPromoBox { get; init; }
}
