using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

public sealed record LogicMapGameSanctuaryAnimalTaskStatePayload(
    int Unknown0,
    int Unknown1,
    int UnknownGlobalId0,
    bool UnknownBoolean0,
    bool UnknownBoolean1,
    bool UnknownBoolean2,
    int Unknown2,
    int Unknown3,
    LogicLong? UnknownPair0) : LogicMapGameTaskStatePayload
{
    internal static LogicMapGameSanctuaryAnimalTaskStatePayload Decode(SupercellStream stream)
    {
        return new LogicMapGameSanctuaryAnimalTaskStatePayload(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            LogicMapGameWire.ReadOptionalLogicLong(stream));
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(UnknownGlobalId0);
        stream.WriteBoolean(UnknownBoolean0);
        stream.WriteBoolean(UnknownBoolean1);
        stream.WriteBoolean(UnknownBoolean2);
        stream.WriteVarInt(Unknown2);
        stream.WriteVarInt(Unknown3);
        LogicMapGameWire.WriteOptionalLogicLong(stream, UnknownPair0);
    }
}
