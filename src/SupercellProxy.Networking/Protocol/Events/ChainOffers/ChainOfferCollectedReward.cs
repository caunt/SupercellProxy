using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Records the first resource granted for a chain step.</summary>
public sealed record ChainOfferCollectedReward
{
    /// <summary>Gets the granted quantity.</summary>
    [JsonPropertyName("Value")]
    public int Amount { get; init; }
    /// <summary>Gets the granted resource identifier.</summary>
    [JsonPropertyName("ID")]
    public int Identifier { get; init; }
}
