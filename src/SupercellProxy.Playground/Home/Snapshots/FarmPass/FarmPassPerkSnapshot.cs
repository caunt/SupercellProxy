namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>FarmPassPerkSnapshot</c> home data.
/// </summary>
public sealed record FarmPassPerkSnapshot
{
    /// <summary>
    /// Gets or sets the <c>PerkDataId</c> value.
    /// </summary>
    public int PerkDataId { get; init; }

    /// <summary>
    /// Gets or sets the <c>Param1</c> value.
    /// </summary>
    public int Param1 { get; init; }

    /// <summary>
    /// Gets or sets the <c>Active</c> value.
    /// </summary>
    public bool Active { get; init; }

    /// <summary>
    /// Gets or sets the <c>PremiumMultiple</c> value.
    /// </summary>
    public int PremiumMultiple { get; init; } = 1;

    /// <summary>
    /// Gets or sets the <c>SubscriptionPerk</c> value.
    /// </summary>
    public bool SubscriptionPerk { get; init; }
}
