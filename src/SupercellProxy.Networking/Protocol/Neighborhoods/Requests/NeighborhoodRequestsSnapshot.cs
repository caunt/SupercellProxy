using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.GameObjects;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Retains goods donated to the player and their collection notification.</summary>
public sealed record NeighborhoodRequestsSnapshot
{
    /// <summary>Gets uncollected donations in native list order.</summary>
    [JsonPropertyName("donationList")]
    public ItemQuantityListSnapshot? DonationList { get; init; }

    /// <summary>Gets whether received donations have a pending notification.</summary>
    [JsonPropertyName("donationReceivedNotification")]
    public bool DonationReceivedNotification { get; init; }
}
