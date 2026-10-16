using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// The avatar-side metadata of one object in an area's layout. Entries with the same data id pair, in order, with the layout's
/// objects of that type.
/// </summary>
public sealed record FarmObjectSnapshot
{
    /// <summary>
    /// Global id of the object's game data entry, such as a building or tree type; it decides which layout objects the entry pairs with.
    /// </summary>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }

    /// <summary>
    /// Whether the object stands on land the player has not expanded into yet; such objects cannot be used, cleared or harvested.
    /// </summary>
    public bool ExpansionLocked { get; init; }
}
