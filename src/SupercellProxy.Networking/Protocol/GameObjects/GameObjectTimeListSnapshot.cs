using System.Collections.Immutable;

using SupercellProxy.Networking.Json;


namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>A saved list of objects kept for one kind of action; its exact role is not identified.</summary>
public sealed record GameObjectTimeListSnapshot
{
    /// <summary>Gets or sets the kind of action the list belongs to; its values are not identified.</summary>
    /// <value>An integer code saved as <c>Action</c> in the home's time lists; the codes and what they select are not yet identified.</value>
    public int Action { get; init; }

    /// <summary>Gets or sets the saved entries of the list, kept as they were received.</summary>
    /// <value>Each entry as raw JSON, not decoded, so it is written back unchanged; empty when the list has no entries.</value>
    public ImmutableArray<EncodedDocumentValue> List { get; init; } = [];
}
