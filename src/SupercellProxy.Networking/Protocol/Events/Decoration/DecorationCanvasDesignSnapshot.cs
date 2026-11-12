using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>A candidate's saved decoration-canvas layout.</summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record DecorationCanvasDesignSnapshot
{
    /// <summary>Gets or sets the horizontal origin of the decorated area.</summary>
    /// <value>The tile X coordinate where the area the player decorated begins, saved as <c>areaPosX</c>.</value>
    [JsonPropertyName("areaPosX")]
    public int AreaPositionX { get; init; }

    /// <summary>Gets or sets the vertical origin of the decorated area.</summary>
    /// <value>The tile Y coordinate where the area the player decorated begins, saved as <c>areaPosY</c>.</value>
    [JsonPropertyName("areaPosY")]
    public int AreaPositionY { get; init; }

    /// <summary>Gets or sets the decorated area's width.</summary>
    /// <value>The number of tiles the area spans along X from <see cref="AreaPositionX"/>.</value>
    [JsonPropertyName("areaSizeX")]
    public int AreaSizeX { get; init; }

    /// <summary>Gets or sets the decorated area's height.</summary>
    /// <value>The number of tiles the area spans along Y from <see cref="AreaPositionY"/>.</value>
    [JsonPropertyName("areaSizeY")]
    public int AreaSizeY { get; init; }

    /// <summary>Gets or sets the canvas identity.</summary>
    /// <value>The id of the decoration-event canvas the design was built on, saved as <c>canvasID</c>.</value>
    [JsonPropertyName("canvasID")]
    public int CanvasId { get; init; }

    /// <summary>Gets or sets the objects arranged on the canvas.</summary>
    /// <value>The layout holding every placed object; an empty layout when the design has none.</value>
    [JsonPropertyName("layout")]
    public DecorationCanvasLayoutSnapshot Layout { get; init; } = new();

    /// <summary>Gets or sets the seasonal theme identity.</summary>
    /// <value>The id of the seasonal theme the design was made for, saved as <c>seasonalThemeID</c>.</value>
    [JsonPropertyName("seasonalThemeID")]
    public int SeasonalThemeId { get; init; }
}
