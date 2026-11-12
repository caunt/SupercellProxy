using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks.Definitions;

/// <summary>
/// The price of replacing a task event task.
/// </summary>
public sealed record TaskEventPurchaseReplacement
{
    /// <summary>
    /// Gets or sets the diamonds a replacement costs.
    /// </summary>
    /// <value>The <c>PurchaseCost</c> in diamonds charged each time the player buys a replacement task; 0 when the document omits it.</value>
    [JsonPropertyName("PurchaseCost")]
    public int PurchaseCost { get; init; }
}
