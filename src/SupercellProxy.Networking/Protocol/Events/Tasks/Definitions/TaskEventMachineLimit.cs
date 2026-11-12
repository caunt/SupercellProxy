using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks.Definitions;

/// <summary>
/// How many tasks for goods of one production building a task event's list may hold.
/// </summary>
public sealed record TaskEventMachineLimit
{
    /// <summary>
    /// Gets or sets the most tasks of the building.
    /// </summary>
    /// <value>The <c>limit</c> on how many tasks for goods of <see cref="Machine"/> the list may hold at the same time.</value>
    [JsonPropertyName("limit")]
    public int Limit { get; init; }

    /// <summary>
    /// Gets or sets the name of the production building's table row.
    /// </summary>
    /// <value>The <c>machine</c> row name of the building the cap applies to; an empty string when the document omits it.</value>
    [JsonPropertyName("machine")]
    public string Machine { get; init; } = string.Empty;
}
