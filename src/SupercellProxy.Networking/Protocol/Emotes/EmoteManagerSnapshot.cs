using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Emotes;

/// Contains emote state that participates in home creation gates.
public sealed record EmoteManagerSnapshot
{
    /// Gets the newest emote identifiers retained by the avatar.
    [JsonPropertyName("newestEmotes")]
    public int[] NewestEmotes { get; init; } = [];
}
