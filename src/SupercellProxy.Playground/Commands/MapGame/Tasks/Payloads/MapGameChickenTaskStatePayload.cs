using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">MapGameChickenTaskStatePayload</c>.
/// </summary>
internal sealed record MapGameChickenTaskStatePayload : MapGameTaskStatePayload
{
    /// <summary>
    /// Initializes a new <see cref="MapGameChickenTaskStatePayload"/> instance.
    /// </summary>
    public MapGameChickenTaskStatePayload(
        int unknown0,
        bool unknownBoolean0,
        LongId? unknownLongId,
        ReadOnlyMemory<CommandDataReferenceVarIntPair>? optionalValues,
        ReadOnlyMemory<LongId> logicLongs
    )
    {
        Unknown0 = unknown0;
        UnknownBoolean0 = unknownBoolean0;
        UnknownLongId = unknownLongId;
        OptionalValues = optionalValues?.ToArray();
        LongIds = logicLongs.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownBoolean0</c> value.
    /// </summary>
    public bool UnknownBoolean0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId</c> value.
    /// </summary>
    public LongId? UnknownLongId { get; }

    /// <summary>
    /// Gets the <c language="csharp">OptionalValues</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair>? OptionalValues { get; }

    /// <summary>
    /// Gets the <c language="csharp">LongIds</c> value.
    /// </summary>
    public ReadOnlyMemory<LongId> LongIds { get; }

    internal static MapGameChickenTaskStatePayload Decode(MessageStream stream)
    {
        return new MapGameChickenTaskStatePayload(
            stream.ReadVarInt(),
            stream.ReadBoolean(),
            MapGameWire.ReadOptionalLongId(stream),
            MapGameWire.ReadOptionalDataReferenceVarIntPairs(stream),
            MapGameWire.ReadLongIds(stream)
        );
    }

    internal override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteBoolean(UnknownBoolean0);
        MapGameWire.WriteOptionalLongId(stream, UnknownLongId);
        MapGameWire.WriteOptionalDataReferenceVarIntPairs(stream, OptionalValues);
        MapGameWire.WriteLongIds(stream, LongIds.Span);
    }
}
