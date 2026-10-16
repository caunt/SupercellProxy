using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>
/// One saved slot of the farm visitor spawner: which visitor is scheduled and when.
/// </summary>
public sealed record PeopleSpawnerSlotSnapshot
{
    /// <summary>
    /// Global id of the visitor in the people table (saved as <c>pid</c>).
    /// </summary>
    [JsonPropertyName("pid")]
    public int PersonGlobalId { get; init; }
    /// <summary>
    /// When the visitor appears, as the spawner's saved time value (saved as <c>st</c>).
    /// </summary>
    [JsonPropertyName("st")]
    public int SpawnTime { get; init; }
}
