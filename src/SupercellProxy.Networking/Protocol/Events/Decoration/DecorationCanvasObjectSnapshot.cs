using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>One object in a decoration-canvas candidate layout.</summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record DecorationCanvasObjectSnapshot
{
    /// <summary>Gets the object's data identity.</summary>
    [JsonPropertyName("D")]
    public int DataGlobalId { get; init; }

    /// <summary>Gets the optional mirror flag.</summary>
    [JsonPropertyName("M")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MirrorFlag { get; init; }

    /// <summary>Gets the horizontal tile position.</summary>
    [JsonPropertyName("X")]
    public int PositionX { get; init; }

    /// <summary>Gets the vertical tile position.</summary>
    [JsonPropertyName("Y")]
    public int PositionY { get; init; }
}
