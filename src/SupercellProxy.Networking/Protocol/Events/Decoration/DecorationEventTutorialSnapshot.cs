using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>
/// Saved progress through the intro screens a decoration event shows the first time the player opens it.
/// </summary>
public sealed record DecorationEventTutorialSnapshot
{
    /// <summary>
    /// Decoration event whose intro the step below belongs to; -1 before any intro was shown.
    /// </summary>
    [JsonPropertyName("lastIntroEventId")]
    public int LastIntroEventId { get; init; } = -1;

    /// <summary>
    /// Last intro screen the player reached; -1 before any intro was shown.
    /// </summary>
    [JsonPropertyName("lastIntroStep")]
    public int LastIntroStep { get; init; } = -1;
}
