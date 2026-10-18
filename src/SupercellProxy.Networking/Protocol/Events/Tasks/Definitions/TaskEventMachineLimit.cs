using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks.Definitions;

/// <summary>
/// How many tasks for goods of one production building a task event's list may hold.
/// </summary>
public sealed record TaskEventMachineLimit
{
    /// <summary>
    /// Most tasks of the building.
    /// </summary>
    [JsonPropertyName("limit")]
    public int Limit { get; init; }

    /// <summary>
    /// Name of the production building's table row.
    /// </summary>
    [JsonPropertyName("machine")]
    public string Machine { get; init; } = string.Empty;
}
