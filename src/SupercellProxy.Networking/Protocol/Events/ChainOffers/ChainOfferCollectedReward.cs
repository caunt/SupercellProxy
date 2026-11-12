using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Records the first resource granted for a chain step.</summary>
public sealed record ChainOfferCollectedReward
{
    /// <summary>Gets or sets the granted quantity.</summary>
    /// <value>How many units of <see cref="Id"/> the claim granted, saved as <c>Value</c>; 0 for a step not claimed yet.</value>
    [JsonPropertyName("Value")]
    public int Amount { get; init; }
    /// <summary>Gets or sets the granted resource id.</summary>
    /// <value>The data global id of the first reward the step's claim granted, saved as <c>ID</c>; 0 for a step not claimed yet.</value>
    [JsonPropertyName("ID")]
    public int Id { get; init; }
}
