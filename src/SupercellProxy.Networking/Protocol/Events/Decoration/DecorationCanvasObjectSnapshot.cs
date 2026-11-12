using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>One object in a decoration-canvas candidate layout.</summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record DecorationCanvasObjectSnapshot
{
    /// <summary>Gets or sets the object's data identity.</summary>
    /// <value>The data-table global id of the placed decoration, saved as <c>D</c>.</value>
    [JsonPropertyName("D")]
    public int DataGlobalId { get; init; }

    /// <summary>Gets or sets the optional mirror flag.</summary>
    /// <value>The saved <c>M</c> value, present when the object was placed mirrored, or <see langword="null"/> when the design omits it, in which case it is also left out on write.</value>
    [JsonPropertyName("M")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MirrorFlag { get; init; }

    /// <summary>Gets or sets the horizontal tile position.</summary>
    /// <value>The object's tile X coordinate on the canvas, saved as <c>X</c>.</value>
    [JsonPropertyName("X")]
    public int PositionX { get; init; }

    /// <summary>Gets or sets the vertical tile position.</summary>
    /// <value>The object's tile Y coordinate on the canvas, saved as <c>Y</c>.</value>
    [JsonPropertyName("Y")]
    public int PositionY { get; init; }
}
