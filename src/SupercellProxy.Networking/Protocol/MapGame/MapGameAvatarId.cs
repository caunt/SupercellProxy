using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>An account or neighbourhood id in saved Valley data, stored as its two 32-bit halves.</summary>
public sealed record MapGameAvatarId : ExtensibleDocument
{
    /// <summary>Gets or sets the upper 32 bits of the id (saved as <c>h</c>).</summary>
    /// <value>The id's high word, which is its shard; 0 by default.</value>
    [JsonPropertyName("h")]
    public int High { get; init; }

    /// <summary>Gets or sets the lower 32 bits of the id (saved as <c>l</c>).</summary>
    /// <value>The id's low word, its number within the shard, stored as a signed integer; 0 by default.</value>
    [JsonPropertyName("l")]
    public int Low { get; init; }
}
