using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks.Definitions;

/// <summary>
/// How a task event replaces a task: <c>Refresh</c> or <c>PurchaseReplacement</c>.
/// </summary>
public sealed record TaskEventReplacement
{
    /// <summary>
    /// Gets or sets the price of a bought replacement, given when <see cref="Type"/> is <c>PurchaseReplacement</c>.
    /// </summary>
    /// <value>The <c>PurchaseReplacement</c> object holding the diamond cost, or <see langword="null"/> when replacements are refreshes rather than purchases.</value>
    [JsonPropertyName("PurchaseReplacement")]
    public TaskEventPurchaseReplacement? PurchaseReplacement { get; init; }

    /// <summary>
    /// Gets or sets the replacement mode, <c>Refresh</c> or <c>PurchaseReplacement</c>.
    /// </summary>
    /// <value>The <c>Type</c> string exactly as the document gives it; <c>PurchaseReplacement</c> makes <see cref="PurchaseReplacement"/> apply, and it is <see langword="null"/> when the document omits it.</value>
    [JsonPropertyName("Type")]
    public string? Type { get; init; }
}
