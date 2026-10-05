namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Identifies a resource granted by a chain step.</summary>
public sealed record ChainOfferRewardDefinition
{
    /// <summary>Gets a decision-box reward.</summary>
    public ChainOfferRewardValue? DecisionBoxes { get; init; }
    /// <summary>Gets a decoration reward.</summary>
    public ChainOfferRewardValue? Decorations { get; init; }
    /// <summary>Gets a money reward.</summary>
    public ChainOfferRewardValue? Money { get; init; }
    /// <summary>Gets an inventory-goods reward.</summary>
    public ChainOfferRewardValue? Products { get; init; }
    /// <summary>Gets a seasonal-currency reward.</summary>
    public ChainOfferRewardValue? SeasonalCurrency { get; init; }
    /// <summary>Gets the native reward category discriminator.</summary>
    public string? Type { get; init; }
}
