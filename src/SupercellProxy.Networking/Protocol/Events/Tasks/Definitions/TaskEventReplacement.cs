using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks.Definitions;

/// <summary>
/// How a task event replaces a task: <c>Refresh</c> or <c>PurchaseReplacement</c>.
/// </summary>
public sealed record TaskEventReplacement
{
    /// <summary>
    /// The price of a bought replacement, given when <see cref="Type"/> is <c>PurchaseReplacement</c>.
    /// </summary>
    [JsonPropertyName("PurchaseReplacement")]
    public TaskEventPurchaseReplacement? PurchaseReplacement { get; init; }

    /// <summary>
    /// <c>Refresh</c> or <c>PurchaseReplacement</c>.
    /// </summary>
    [JsonPropertyName("Type")]
    public string? Type { get; init; }
}
