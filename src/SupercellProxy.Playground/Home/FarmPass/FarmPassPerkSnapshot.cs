namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">FarmPassPerkSnapshot</c> home data.
/// </summary>
internal sealed record FarmPassPerkSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">PerkDataId</c> value.
    /// </summary>
    public int PerkDataId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Param1</c> value.
    /// </summary>
    public int Param1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Active</c> value.
    /// </summary>
    public bool Active { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PremiumMultiple</c> value.
    /// </summary>
    public int PremiumMultiple { get; init; } = 1;

    /// <summary>
    /// Gets or sets the <c language="csharp">SubscriptionPerk</c> value.
    /// </summary>
    public bool SubscriptionPerk { get; init; }
}
