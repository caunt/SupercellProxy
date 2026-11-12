using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// The avatar-side metadata of one object in an area's layout. Entries with the same data id pair, in order, with the layout's
/// objects of that type.
/// </summary>
public sealed record FarmObjectSnapshot
{
    /// <summary>
    /// Gets or sets the global id of the object's game data entry, such as a building or tree type; it decides which layout objects the entry pairs with.
    /// </summary>
    /// <value>The data global id read from the JSON <c>ID</c> property; 0 when the property is missing, which pairs with no layout object.</value>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the object stands on land the player has not expanded into yet.
    /// </summary>
    /// <value><see langword="true"/> if the object lies in a locked expansion area; otherwise, <see langword="false"/>.</value>
    /// <remarks>A save that omits the flag reads as <see langword="false"/>.</remarks>
    public bool ExpansionLocked { get; init; }
}
