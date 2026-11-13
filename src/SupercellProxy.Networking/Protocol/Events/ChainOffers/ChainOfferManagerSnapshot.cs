using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Contains the player's saved chain-offer progress.</summary>
public sealed record ChainOfferManagerSnapshot
{
    /// <summary>Gets or sets the retained offer states.</summary>
    /// <value>One state per chain-offer event the player has progress in, saved as <c>offers</c>; empty by default.</value>
    [JsonPropertyName("offers")]
    public ImmutableArray<ChainOfferSnapshot> Offers { get; init; } = [];
}
