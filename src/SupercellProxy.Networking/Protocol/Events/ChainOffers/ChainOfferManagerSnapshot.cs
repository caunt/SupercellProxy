using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Contains the player's saved chain-offer progress.</summary>
public sealed record ChainOfferManagerSnapshot
{
    /// <summary>Gets the retained offer states.</summary>
    [JsonPropertyName("offers")]
    public ChainOfferSnapshot[] Offers { get; init; } = [];
}
