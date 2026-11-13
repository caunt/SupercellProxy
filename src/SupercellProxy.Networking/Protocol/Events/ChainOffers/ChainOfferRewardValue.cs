using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Names a chain reward and its quantity.</summary>
public sealed record ChainOfferRewardValue
{
    /// <summary>Gets or sets the reward quantity.</summary>
    /// <value>How many units of the named resource the step grants.</value>
    public int Amount { get; init; }

    /// <summary>Gets or sets the resource's asset name.</summary>
    /// <value>The row name of the granted resource in the data table chosen by the reward's category (for example a product, booster or decoration name), or <see langword="null"/> when the event payload omits it.</value>
    [JsonPropertyName("data")]
    public string? Data { get; init; }
}
