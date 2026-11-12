using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks.Definitions;

/// <summary>
/// Whether a task event limits how many tasks a player gets: <c>UnlimitedTasks</c> or <c>LimitedTasks</c>.
/// </summary>
public sealed record TaskEventCountType
{
    /// <summary>
    /// Gets or sets the daily quota, given when <see cref="Type"/> is <c>LimitedTasks</c>.
    /// </summary>
    /// <value>The <c>LimitedTasks</c> object with the daily task count and the prices of extra slots, or <see langword="null"/> for an unlimited event.</value>
    [JsonPropertyName("LimitedTasks")]
    public TaskEventLimitedTasks? LimitedTasks { get; init; }

    /// <summary>
    /// Gets or sets the quota mode, <c>UnlimitedTasks</c> or <c>LimitedTasks</c>.
    /// </summary>
    /// <value>The <c>Type</c> string exactly as the document gives it; only <c>LimitedTasks</c> makes <see cref="LimitedTasks"/> apply, and it is <see langword="null"/> when the document omits it.</value>
    [JsonPropertyName("Type")]
    public string? Type { get; init; }
}
