using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.GameObjects;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Retains goods donated to the player and their collection notification.</summary>
public sealed record NeighborhoodRequestsSnapshot
{
    /// <summary>Gets or sets the uncollected donations in native list order.</summary>
    /// <value>
    /// Saved as <c>donationList</c>; parallel item id and amount arrays, one entry per donation waiting to be collected, or
    /// <see langword="null"/> when nothing was ever donated.
    /// </value>
    [JsonPropertyName("donationList")]
    public ItemQuantityListSnapshot? DonationList { get; init; }

    /// <summary>Gets or sets whether received donations have a pending notification.</summary>
    /// <value>
    /// <see langword="true"/> if a donation arrived since the notification was last cleared; otherwise, <see langword="false"/>.
    /// </value>
    [JsonPropertyName("donationReceivedNotification")]
    public bool DonationReceivedNotification { get; init; }
}
