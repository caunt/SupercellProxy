using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>
/// Saved progress through the intro screens a decoration event shows the first time the player opens it.
/// </summary>
public sealed record DecorationEventTutorialSnapshot
{
    /// <summary>
    /// Gets or sets the decoration event whose intro the step below belongs to; -1 before any intro was shown.
    /// </summary>
    /// <value>The live event id that was running when the last intro step was recorded.</value>
    [JsonPropertyName("lastIntroEventId")]
    public int LastIntroEventId { get; init; } = -1;

    /// <summary>
    /// Gets or sets the last intro screen the player reached; -1 before any intro was shown.
    /// </summary>
    /// <value>The zero-based intro screen index sent by the latest tutorial command for <see cref="LastIntroEventId"/>.</value>
    [JsonPropertyName("lastIntroStep")]
    public int LastIntroStep { get; init; } = -1;
}
