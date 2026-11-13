using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks.Definitions;

/// <summary>
/// The daily task quota of a task event.
/// </summary>
public sealed record TaskEventLimitedTasks
{
    /// <summary>
    /// Gets or sets the number of new tasks a player gets each day.
    /// </summary>
    /// <value>The <c>Count</c> of new tasks per event day; 0 when the document omits it.</value>
    [JsonPropertyName("Count")]
    public int Count { get; init; }

    /// <summary>
    /// Gets or sets the diamond prices of the extra task slots bought in one day, in purchase order.
    /// </summary>
    /// <value>The <c>TaskQueuePurchaseCosts</c> list, where the first entry is the price of the day's first extra slot; an empty array by default.</value>
    [JsonPropertyName("TaskQueuePurchaseCosts")]
    public ImmutableArray<int> TaskQueuePurchaseCosts { get; init; } = [];
}
