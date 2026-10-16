using SupercellProxy.Networking.Json;


namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>A saved list of objects kept for one kind of action; its exact role is not identified.</summary>
public sealed record GameObjectTimeListSnapshot
{
    /// <summary>Kind of action the list belongs to; its values are not identified.</summary>
    public int Action { get; init; }

    /// <summary>Saved entries of the list, kept as they were received.</summary>
    public EncodedDocumentValue[] List { get; init; } = [];
}
