using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Emotes;

/// <summary>Contains emote state that participates in home creation gates.</summary>
public sealed record EmoteManagerSnapshot
{
    /// <summary>Gets the newest emote ids retained by the avatar.</summary>
    [JsonPropertyName("newestEmotes")]
    public int[] NewestEmotes { get; init; } = [];
}
