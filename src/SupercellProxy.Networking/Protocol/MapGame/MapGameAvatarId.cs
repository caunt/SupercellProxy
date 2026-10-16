using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>An account or neighbourhood id in saved Valley data, stored as its two 32-bit halves.</summary>
public sealed record MapGameAvatarId : ExtensibleDocument
{
    /// <summary>Upper 32 bits of the id (saved as <c>h</c>).</summary>
    [JsonPropertyName("h")]
    public int High { get; init; }

    /// <summary>Lower 32 bits of the id (saved as <c>l</c>).</summary>
    [JsonPropertyName("l")]
    public int Low { get; init; }
}
