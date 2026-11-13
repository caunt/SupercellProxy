using System.Collections.Immutable;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Defines one chain reward and its prerequisites.</summary>
public sealed record ChainOfferStepDefinition
{
    /// <summary>Gets or sets the optional payment.</summary>
    /// <value>The purchase category of a paid step, or <see langword="null"/> for a free step.</value>
    public ChainOfferCostDefinition? Cost { get; init; }

    /// <summary>Gets or sets the step's requirements.</summary>
    /// <value>The step's time-since-start and farm-task requirements; empty by default.</value>
    public ImmutableArray<ChainOfferDependencyEntry> Dependencies { get; init; } = [];

    /// <summary>Gets or sets all reward groups granted by this step.</summary>
    /// <value>One group per reward of the step, each holding a primary reward and its fallbacks; empty by default.</value>
    public ImmutableArray<ChainOfferRewardGroup> Rewards { get; init; } = [];
}
