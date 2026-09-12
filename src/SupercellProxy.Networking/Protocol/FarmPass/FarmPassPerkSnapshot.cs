namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>
/// Represents decoded <c language="csharp">FarmPassPerkSnapshot</c> home data.
/// </summary>
public sealed record FarmPassPerkSnapshot
{

    /// <summary>
    /// Gets or sets the <c language="csharp">Active</c> value.
    /// </summary>
    public bool Active { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Param1</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Param1")]
    public int Parameter1 { get; init; }
    /// <summary>
    /// Gets or sets the <c language="csharp">PerkDataId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("PerkDataId")]
    public int PerkDataIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PremiumMultiple</c> value.
    /// </summary>
    public int PremiumMultiple { get; init; } = 1;

    /// <summary>
    /// Gets or sets the <c language="csharp">SubscriptionPerk</c> value.
    /// </summary>
    public bool SubscriptionPerk { get; init; }
}
