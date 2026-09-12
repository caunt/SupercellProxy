using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>
/// Defines the Decoration Event Tutorial Snapshot contract.
/// </summary>
public sealed record DecorationEventTutorialSnapshot
{
    /// <summary>
    /// Gets the Last Intro Event Id value.
    /// </summary>
    [JsonPropertyName("lastIntroEventId")]
    public int LastIntroEventIdentifier { get; init; } = -1;

    /// <summary>
    /// Gets the Last Intro Step value.
    /// </summary>
    [JsonPropertyName("lastIntroStep")]
    public int LastIntroStep { get; init; } = -1;
}
