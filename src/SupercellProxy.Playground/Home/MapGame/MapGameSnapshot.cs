using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">MapGameSnapshot</c> home data.
/// </summary>
internal sealed record MapGameSnapshot
{
    /// <summary>
    /// Gets the retained <c language="csharp">MapGameManager</c> state.
    /// </summary>
    [JsonPropertyName("MapGameManager")]
    public JsonElement Manager { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">MapGlobalId</c> value.
    /// </summary>
    public int MapGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">QuestrManager</c> value.
    /// </summary>
    public MapGameQuestSnapshot QuestrManager { get; init; } = new();
}
