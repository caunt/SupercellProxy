using SupercellProxy.Networking.Protocol.Boats;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Rewards;

/// <summary>Preserves a promotion reward attached to a derby choice.</summary>
public sealed record DerbyPromotionRewardSnapshot
{
    /// <summary>Gets the flattened ordinary choice index.</summary>
    public int PopBoxThresholdIndex { get; init; }
    /// <summary>Gets the shared promotion reward payload.</summary>
    public BoatPromotionRewardSnapshot? PopPromoBox { get; init; }
}
