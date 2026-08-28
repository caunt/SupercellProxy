using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">PeopleSpawnerSlotSnapshot</c> home data.
/// </summary>
internal sealed record PeopleSpawnerSlotSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">SpawnTime</c> value.
    /// </summary>
    [JsonPropertyName("st")]
    public int SpawnTime { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PersonGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("pid")]
    public int PersonGlobalId { get; init; }
}
