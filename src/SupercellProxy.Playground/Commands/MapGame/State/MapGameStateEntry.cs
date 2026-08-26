using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Native data-reference entry inside a map-game state.</para>
/// </summary>
public sealed record MapGameStateEntry
{
    /// <summary>
    /// Initializes a new <see cref="MapGameStateEntry"/> instance.
    /// </summary>
    public MapGameStateEntry(
        int unknownGlobalId,
        int unknown0,
        LongId? unknownLongId,
        int unknown1,
        int unknown2,
        ReadOnlyMemory<LongId> unknownLongIds
    )
    {
        UnknownGlobalId = unknownGlobalId;
        Unknown0 = unknown0;
        UnknownLongId = unknownLongId;
        Unknown1 = unknown1;
        Unknown2 = unknown2;
        UnknownLongIds = unknownLongIds.ToArray();
    }

    /// <summary>
    /// Gets the <c>UnknownGlobalId</c> value.
    /// </summary>
    public int UnknownGlobalId { get; }

    /// <summary>
    /// Gets the <c>Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c>UnknownLongId</c> value.
    /// </summary>
    public LongId? UnknownLongId { get; }

    /// <summary>
    /// Gets the <c>Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; }

    /// <summary>
    /// Gets the <c>Unknown2</c> value.
    /// </summary>
    public int Unknown2 { get; }

    /// <summary>
    /// Gets the <c>UnknownLongIds</c> value.
    /// </summary>
    public ReadOnlyMemory<LongId> UnknownLongIds { get; }

    internal static MapGameStateEntry Decode(MessageStream stream)
    {
        return new MapGameStateEntry(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            MapGameWire.ReadOptionalLongId(stream),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            MapGameWire.ReadLongIds(stream)
        );
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(UnknownGlobalId);
        stream.WriteVarInt(Unknown0);
        MapGameWire.WriteOptionalLongId(stream, UnknownLongId);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        MapGameWire.WriteLongIds(stream, UnknownLongIds.Span);
    }
}
