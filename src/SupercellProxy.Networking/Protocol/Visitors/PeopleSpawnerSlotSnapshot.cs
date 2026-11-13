using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>
/// One saved slot of the farm visitor spawner: which visitor is scheduled and when.
/// </summary>
public sealed record PeopleSpawnerSlotSnapshot
{
    /// <summary>
    /// Gets or sets the global id of the visitor in the people table (saved as <c>pid</c>).
    /// </summary>
    /// <value>Data global id of the people row occupying this slot, or 0 for an empty slot waiting to spawn.</value>
    [JsonPropertyName("pid")]
    public int PersonGlobalId { get; init; }

    /// <summary>
    /// Gets or sets when the visitor appears, as the spawner's saved time value (saved as <c>st</c>).
    /// </summary>
    /// <value>Server timestamp in seconds at which an empty slot spawns its next visitor; 0 means unscheduled.</value>
    [JsonPropertyName("st")]
    public int SpawnTime { get; init; }
}
