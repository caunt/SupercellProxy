using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>
/// Represents decoded <c language="csharp">PeopleSpawnerSlotSnapshot</c> home data.
/// </summary>
public sealed record PeopleSpawnerSlotSnapshot
{

    /// <summary>
    /// Gets or sets the <c language="csharp">PersonGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("pid")]
    public int PersonGlobalIdentifier { get; init; }
    /// <summary>
    /// Gets or sets the <c language="csharp">SpawnTime</c> value.
    /// </summary>
    [JsonPropertyName("st")]
    public int SpawnTime { get; init; }
}
