using SupercellProxy.Networking.Protocol.Boats;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Rewards;

/// <summary>Preserves a promotion reward attached to a derby choice.</summary>
public sealed record DerbyPromotionRewardSnapshot
{
    /// <summary>Gets or sets the flattened ordinary choice index.</summary>
    /// <value>The zero-based position, counted across all thresholds and their slots in order, of the ordinary choice that carries the Pop box.</value>
    public int PopBoxThresholdIndex { get; init; }

    /// <summary>Gets or sets the shared promotion reward payload.</summary>
    /// <value>The Pop box's reward and collected marker, in the same form boat crates use, or <see langword="null"/> when the save has no box object.</value>
    public BoatPromotionRewardSnapshot? PopPromoBox { get; init; }
}
