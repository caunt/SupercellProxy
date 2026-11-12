using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Emotes;

/// <summary>Contains emote state that participates in home creation gates.</summary>
public sealed record EmoteManagerSnapshot
{
    /// <summary>Gets or sets the newest emote ids retained by the avatar.</summary>
    /// <value>Emote ids saved as <c>newestEmotes</c>, in saved order; how native uses them beyond the name is not identified, nothing in this library interprets them, and the list is empty by default.</value>
    [JsonPropertyName("newestEmotes")]
    public int[] NewestEmotes { get; init; } = [];
}
