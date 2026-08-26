using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>MapGameSanctuaryAnimalTaskStatePayload</c>.
/// </summary>
/// <param name="Unknown0">The <c>Unknown0</c> value.</param>
/// <param name="Unknown1">The <c>Unknown1</c> value.</param>
/// <param name="UnknownGlobalId0">The <c>UnknownGlobalId0</c> value.</param>
/// <param name="UnknownBoolean0">The <c>UnknownBoolean0</c> value.</param>
/// <param name="UnknownBoolean1">The <c>UnknownBoolean1</c> value.</param>
/// <param name="UnknownBoolean2">The <c>UnknownBoolean2</c> value.</param>
/// <param name="Unknown2">The <c>Unknown2</c> value.</param>
/// <param name="Unknown3">The <c>Unknown3</c> value.</param>
/// <param name="UnknownPair0">The <c>UnknownPair0</c> value.</param>
public sealed record MapGameSanctuaryAnimalTaskStatePayload(
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
