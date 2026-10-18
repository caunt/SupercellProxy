using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks.Definitions;

/// <summary>
/// The price of replacing a task event task.
/// </summary>
public sealed record TaskEventPurchaseReplacement
{
    /// <summary>
    /// Diamonds a replacement costs.
    /// </summary>
    [JsonPropertyName("PurchaseCost")]
    public int PurchaseCost { get; init; }
}
