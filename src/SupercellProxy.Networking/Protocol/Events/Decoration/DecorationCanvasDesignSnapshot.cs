using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>A candidate's saved decoration-canvas layout.</summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record DecorationCanvasDesignSnapshot
{
    /// <summary>Gets the horizontal origin of the decorated area.</summary>
    [JsonPropertyName("areaPosX")]
    public int AreaPositionX { get; init; }

    /// <summary>Gets the vertical origin of the decorated area.</summary>
    [JsonPropertyName("areaPosY")]
    public int AreaPositionY { get; init; }

    /// <summary>Gets the decorated area's width.</summary>
    [JsonPropertyName("areaSizeX")]
    public int AreaSizeX { get; init; }

    /// <summary>Gets the decorated area's height.</summary>
    [JsonPropertyName("areaSizeY")]
    public int AreaSizeY { get; init; }

    /// <summary>Gets the canvas identity.</summary>
    [JsonPropertyName("canvasID")]
    public int CanvasIdentifier { get; init; }

    /// <summary>Gets the objects arranged on the canvas.</summary>
    [JsonPropertyName("layout")]
    public DecorationCanvasLayoutSnapshot Layout { get; init; } = new();

    /// <summary>Gets the seasonal theme identity.</summary>
    [JsonPropertyName("seasonalThemeID")]
    public int SeasonalThemeIdentifier { get; init; }
}
