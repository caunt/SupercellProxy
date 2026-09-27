using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Objects placed in one decoration-canvas layout.</summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record DecorationCanvasLayoutSnapshot
{
    /// <summary>Gets the placed canvas objects.</summary>
    public DecorationCanvasObjectSnapshot[] Objects { get; init; } = [];
}
