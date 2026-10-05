namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Defines one chain reward and its prerequisites.</summary>
public sealed record ChainOfferStepDefinition
{
    /// <summary>Gets the optional payment. Chores excludes every step with a payment definition.</summary>
    public ChainOfferCostDefinition? Cost { get; init; }
    /// <summary>Gets the requirements that must all be satisfied.</summary>
    public ChainOfferDependencyEntry[] Dependencies { get; init; } = [];
    /// <summary>Gets all reward groups granted by this step.</summary>
    public ChainOfferRewardGroup[] Rewards { get; init; } = [];
}
