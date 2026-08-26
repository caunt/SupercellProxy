using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>PeopleSpawnerSlotSnapshot</c> home data.
/// </summary>
public sealed record PeopleSpawnerSlotSnapshot
{
    /// <summary>
    /// Gets or sets the <c>SpawnTime</c> value.
    /// </summary>
    [JsonPropertyName("st")]
    public int SpawnTime { get; init; }

    /// <summary>
    /// Gets or sets the <c>PersonGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("pid")]
    public int PersonGlobalId { get; init; }
}
