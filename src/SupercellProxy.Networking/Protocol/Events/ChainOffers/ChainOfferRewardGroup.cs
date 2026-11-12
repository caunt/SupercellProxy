namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Defines a primary reward and its ordered eligibility fallbacks.</summary>
public sealed record ChainOfferRewardGroup
{
    /// <summary>Gets or sets the first fallback.</summary>
    /// <value>The alternative listed after <see cref="Primary"/>; <see langword="null"/> when there is none.</value>
    public ChainOfferRewardDefinition? Fallback1 { get; init; }
    /// <summary>Gets or sets the second fallback.</summary>
    /// <value>The alternative listed after <see cref="Fallback1"/>; <see langword="null"/> when there is none.</value>
    public ChainOfferRewardDefinition? Fallback2 { get; init; }
    /// <summary>Gets or sets the third fallback.</summary>
    /// <value>A further alternative listed after <see cref="Fallback2"/> in the event document; <see langword="null"/> when there is none.</value>
    public ChainOfferRewardDefinition? Fallback3 { get; init; }
    /// <summary>Gets or sets the primary reward.</summary>
    /// <value>The reward listed first in the group, or <see langword="null"/> when the group has none.</value>
    public ChainOfferRewardDefinition? Primary { get; init; }
}
