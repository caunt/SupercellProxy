using SupercellProxy.Networking.Protocol.MapGame.Quests;

using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// Represents decoded <c language="csharp">MapGameSnapshot</c> home data.
/// </summary>
public sealed record MapGameSnapshot
{
    /// <summary>
    /// Gets the Event value.
    /// </summary>
    public int Event { get; init; }

    /// <summary>
    /// Gets the retained <c language="csharp">MapGameManager</c> state.
    /// </summary>
    [JsonPropertyName("MapGameManager")]
    public MapGameHomeManagerSnapshot? Manager { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">MapGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("MapGlobalId")]
    public int MapGlobalIdentifier { get; init; }

    /// <summary>
    /// Gets the Next Expire Hour Index value.
    /// </summary>
    public int NextExpireHourIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">QuestrManager</c> value.
    /// </summary>
    public MapGameQuestSnapshot QuestrManager { get; init; } = new();
}
