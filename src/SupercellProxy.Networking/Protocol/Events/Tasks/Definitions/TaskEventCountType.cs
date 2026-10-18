using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks.Definitions;

/// <summary>
/// Whether a task event limits how many tasks a player gets: <c>UnlimitedTasks</c> or <c>LimitedTasks</c>.
/// </summary>
public sealed record TaskEventCountType
{
    /// <summary>
    /// The daily quota, given when <see cref="Type"/> is <c>LimitedTasks</c>.
    /// </summary>
    [JsonPropertyName("LimitedTasks")]
    public TaskEventLimitedTasks? LimitedTasks { get; init; }

    /// <summary>
    /// <c>UnlimitedTasks</c> or <c>LimitedTasks</c>.
    /// </summary>
    [JsonPropertyName("Type")]
    public string? Type { get; init; }
}
