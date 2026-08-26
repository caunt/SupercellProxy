using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>MapGameDumpTaskStatePayload</c>.
/// </summary>
public sealed record MapGameDumpTaskStatePayload : MapGameTaskStatePayload
{
    /// <summary>
    /// Initializes a new <see cref="MapGameDumpTaskStatePayload"/> instance.
    /// </summary>
    public MapGameDumpTaskStatePayload(
        ReadOnlyMemory<CommandDataReferenceVarIntPair> values,
        ReadOnlyMemory<CommandDataReferenceVarIntPair>? optionalValues,
        bool unknown0,
        LongId? unknownLongId,
        int unknownGlobalId0,
        int unknownGlobalId1
    )
    {
        Values = values.ToArray();
        OptionalValues = optionalValues?.ToArray();
        Unknown0 = unknown0;
        UnknownLongId = unknownLongId;
        UnknownGlobalId0 = unknownGlobalId0;
        UnknownGlobalId1 = unknownGlobalId1;
    }

    /// <summary>
    /// Gets the <c>Values</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> Values { get; }

    /// <summary>
    /// Gets the <c>OptionalValues</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair>? OptionalValues { get; }

    /// <summary>
    /// Gets the <c>Unknown0</c> value.
    /// </summary>
    public bool Unknown0 { get; }

    /// <summary>
    /// Gets the <c>UnknownLongId</c> value.
    /// </summary>
    public LongId? UnknownLongId { get; }

    /// <summary>
    /// Gets the <c>UnknownGlobalId0</c> value.
    /// </summary>
    public int UnknownGlobalId0 { get; }

    /// <summary>
    /// Gets the <c>UnknownGlobalId1</c> value.
    /// </summary>
    public int UnknownGlobalId1 { get; }

    internal static MapGameDumpTaskStatePayload Decode(MessageStream stream)
    {
        return new MapGameDumpTaskStatePayload(
            MapGameWire.ReadDataReferenceVarIntPairs(stream),
            MapGameWire.ReadOptionalDataReferenceVarIntPairs(stream),
            stream.ReadBoolean(),
            MapGameWire.ReadOptionalLongId(stream),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );
    }

    internal override void Encode(MessageStream stream)
    {
        MapGameWire.WriteDataReferenceVarIntPairs(stream, Values.Span);
        MapGameWire.WriteOptionalDataReferenceVarIntPairs(stream, OptionalValues);
        stream.WriteBoolean(Unknown0);
        MapGameWire.WriteOptionalLongId(stream, UnknownLongId);
        stream.WriteVarInt(UnknownGlobalId0);
        stream.WriteVarInt(UnknownGlobalId1);
    }
}
