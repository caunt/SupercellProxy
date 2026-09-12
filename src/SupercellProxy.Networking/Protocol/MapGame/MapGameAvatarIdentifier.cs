using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Represents decoded MapGameAvatarIdentifier state.</summary>
public sealed record MapGameAvatarIdentifier : ExtensibleDocument
{
    /// <summary>Gets the High value.</summary>
    [JsonPropertyName("h")]
    public int High { get; init; }

    /// <summary>Gets the Low value.</summary>
    [JsonPropertyName("l")]
    public int Low { get; init; }

}
