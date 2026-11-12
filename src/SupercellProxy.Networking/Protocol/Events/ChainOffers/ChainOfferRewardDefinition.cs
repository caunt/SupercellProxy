namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Identifies a resource granted by a chain step.</summary>
public sealed record ChainOfferRewardDefinition
{
    /// <summary>Gets or sets a named booster reward.</summary>
    /// <value>A booster name from the boosters table and its amount, or <see langword="null"/> when the reward is of another category.</value>
    public ChainOfferRewardValue? Boosters { get; init; }
    /// <summary>Gets or sets a decision-box reward.</summary>
    /// <value>A decision box name and its amount, or <see langword="null"/> when the reward is of another category.</value>
    public ChainOfferRewardValue? DecisionBoxes { get; init; }
    /// <summary>Gets or sets a decoration reward.</summary>
    /// <value>A decoration, fence, modular decoration or ditch name and its amount, or <see langword="null"/> when the reward is of another category.</value>
    public ChainOfferRewardValue? Decorations { get; init; }
    /// <summary>Gets or sets an emote reward.</summary>
    /// <value>An emote name from the emotes table and its amount, or <see langword="null"/> when the reward is of another category.</value>
    public ChainOfferRewardValue? Emotes { get; init; }
    /// <summary>Gets or sets a Farm Pass perk reward.</summary>
    /// <value>A Farm Pass perk name and its amount, or <see langword="null"/> when the reward is of another category.</value>
    public ChainOfferRewardValue? FarmPassPerks { get; init; }
    /// <summary>Gets or sets a money reward.</summary>
    /// <value>A money resource name such as coins or diamonds and its amount, or <see langword="null"/> when the reward is of another category.</value>
    public ChainOfferRewardValue? Money { get; init; }
    /// <summary>Gets or sets an inventory-goods reward.</summary>
    /// <value>A product name and its amount, or <see langword="null"/> when the reward is of another category.</value>
    public ChainOfferRewardValue? Products { get; init; }
    /// <summary>Gets or sets a seasonal-currency reward.</summary>
    /// <value>A seasonal currency name and its amount, or <see langword="null"/> when the reward is of another category.</value>
    public ChainOfferRewardValue? SeasonalCurrency { get; init; }
    /// <summary>Gets or sets the native reward category discriminator.</summary>
    /// <value>The category name as the event document writes it, or <see langword="null"/>.</value>
    public string? Type { get; init; }
}
