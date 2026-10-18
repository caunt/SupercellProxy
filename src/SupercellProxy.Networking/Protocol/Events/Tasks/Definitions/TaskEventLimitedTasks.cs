using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks.Definitions;

/// <summary>
/// The daily task quota of a task event.
/// </summary>
public sealed record TaskEventLimitedTasks
{
    /// <summary>
    /// New tasks a player gets each day.
    /// </summary>
    [JsonPropertyName("Count")]
    public int Count { get; init; }

    /// <summary>
    /// Diamond prices of the extra task slots bought in one day, in purchase order; the last price repeats.
    /// </summary>
    [JsonPropertyName("TaskQueuePurchaseCosts")]
    public int[] TaskQueuePurchaseCosts { get; init; } = [];
}
