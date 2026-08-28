using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">MapGameSanctuaryAnimalTaskStatePayload</c>.
/// </summary>
/// <param name="Unknown0">The <c language="csharp">Unknown0</c> value.</param>
/// <param name="Unknown1">The <c language="csharp">Unknown1</c> value.</param>
/// <param name="UnknownGlobalId0">The <c language="csharp">UnknownGlobalId0</c> value.</param>
/// <param name="UnknownBoolean0">The <c language="csharp">UnknownBoolean0</c> value.</param>
/// <param name="UnknownBoolean1">The <c language="csharp">UnknownBoolean1</c> value.</param>
/// <param name="UnknownBoolean2">The <c language="csharp">UnknownBoolean2</c> value.</param>
/// <param name="Unknown2">The <c language="csharp">Unknown2</c> value.</param>
/// <param name="Unknown3">The <c language="csharp">Unknown3</c> value.</param>
/// <param name="UnknownPair0">The <c language="csharp">UnknownPair0</c> value.</param>
internal sealed record MapGameSanctuaryAnimalTaskStatePayload(
    int Unknown0,
    int Unknown1,
    int UnknownGlobalId0,
    bool UnknownBoolean0,
    bool UnknownBoolean1,
    bool UnknownBoolean2,
    int Unknown2,
    int Unknown3,
    LongId? UnknownPair0
) : MapGameTaskStatePayload
{
    internal static MapGameSanctuaryAnimalTaskStatePayload Decode(MessageStream stream)
    {
        return new MapGameSanctuaryAnimalTaskStatePayload(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            MapGameWire.ReadOptionalLongId(stream)
        );
    }

    internal override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(UnknownGlobalId0);
        stream.WriteBoolean(UnknownBoolean0);
        stream.WriteBoolean(UnknownBoolean1);
        stream.WriteBoolean(UnknownBoolean2);
        stream.WriteVarInt(Unknown2);
        stream.WriteVarInt(Unknown3);
        MapGameWire.WriteOptionalLongId(stream, UnknownPair0);
    }
}
