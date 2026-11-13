using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Objects placed in one decoration-canvas layout.</summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record DecorationCanvasLayoutSnapshot
{
    /// <summary>Gets or sets the placed canvas objects.</summary>
    /// <value>Each decoration placed on the canvas with its data id and tile position, in saved order; empty for a blank canvas.</value>
    public ImmutableArray<DecorationCanvasObjectSnapshot> Objects { get; init; } = [];
}
