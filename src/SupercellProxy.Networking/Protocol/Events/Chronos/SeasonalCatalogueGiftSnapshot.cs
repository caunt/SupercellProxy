using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Defines the Seasonal Catalogue Gift Snapshot contract.
/// </summary>
public sealed record SeasonalCatalogueGiftSnapshot
{

    /// <summary>
    /// Gets the Payment Amount value.
    /// </summary>
    [JsonPropertyName("pa")]
    public int PaymentAmount { get; init; }

    /// <summary>
    /// Gets the Payment Global Id value.
    /// </summary>
    [JsonPropertyName("pd")]
    public int PaymentGlobalIdentifier { get; init; }

    /// <summary>
    /// Gets the Purchase Limit value.
    /// </summary>
    [JsonPropertyName("ntp")]
    public int PurchaseLimit { get; init; }

    /// <summary>
    /// Gets the Reward Amount value.
    /// </summary>
    [JsonPropertyName("a")]
    public int RewardAmount { get; init; }
    /// <summary>
    /// Gets the Reward Global Id value.
    /// </summary>
    [JsonPropertyName("d")]
    public int RewardGlobalIdentifier { get; init; }
}
