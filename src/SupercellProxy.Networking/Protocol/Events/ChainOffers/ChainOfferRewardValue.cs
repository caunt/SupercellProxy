using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Names a chain reward and its quantity.</summary>
public sealed record ChainOfferRewardValue
{
    /// <summary>Gets the reward quantity.</summary>
    public int Amount { get; init; }
    /// <summary>Gets the resource's asset name.</summary>
    [JsonPropertyName("data")]
    public string? Data { get; init; }
}
